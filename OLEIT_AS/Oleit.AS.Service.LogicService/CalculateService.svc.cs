using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.PeriodAccessReference;
using Oleit.AS.Service.LogicService.RecordAccessReference;
using Oleit.AS.Service.LogicService.UserAccessReference;
using Oleit.AS.Service.LogicService.WeeklySummaryReference;
using Oleit.AS.Service.LogicService.EntityAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CalculateService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CalculateService.svc or CalculateService.svc.cs at the Solution Explorer and start debugging.
    public class CalculateService : ICalculateService
    {
        public static volatile CalculateService Instance = new CalculateService();

        public void DoWork()
        {
        }

        public JournalCollection AutoJournal(JournalCollection pAndLEntities)
        {
            JournalCollection _result = new JournalCollection();

            foreach (Journal _pAndLEntity in pAndLEntities)
            {
                _pAndLEntity.BaseAmount = _pAndLEntity.SGDAmount * _pAndLEntity.ExchangeRate;

                RelationCollection _relationCollection = EntityService.Instance.RelationEntityWandL(_pAndLEntity.EntityID);
                if (_relationCollection.Count == 0)
                {
                    continue;
                }

                Relation _allocate = _relationCollection.FirstOrDefault(relation => ((int)relation.Description)<4);
                IEnumerable<Relation> _positions = _relationCollection.Where(relation => (relation.Description == RelationDescription.Position) && !pAndLEntities.Any(journal=>journal.EntityID == relation.TargetEntity.EntityID) );
                //if the node was selected then it didn't need to be selected again.
                //!pAndLEntities.Any(journal=>journal.EntityID == relation.TargetEntity.EntityID 

                if (_allocate == null)
                {
                    continue;
                }
                if (!pAndLEntities.Any(Journal => (Journal.EntityID == _allocate.TargetEntity.EntityID)))
                {
                    Journal _journalAllocate = new Journal()
                    {
                        RecordID = _pAndLEntity.RecordID,
                        EntityID = _allocate.TargetEntity.EntityID,                      
                        BaseCurrency = _allocate.TargetEntity.Currency.CurrencyID,
                        ExchangeRate = _allocate.TargetEntity.ExchangeRate,
                        SequenceNo = _pAndLEntity.SequenceNo,

                        //-600 = 550 / 0.55 * 0.6 * -1
                        //600 = -550 / 0.55 * 0.6 * -1
                        SGDAmount = (_pAndLEntity.SGDAmount / _allocate.Numeric * (_allocate.Numeric + _positions.Sum(_position => _position.Numeric)) * -1).ExtRound(),

                        EntryUser = _pAndLEntity.EntryUser,
                    };
               
                _journalAllocate.BaseAmount = (_journalAllocate.SGDAmount * _journalAllocate.ExchangeRate).ExtRound();
                _result.Add(_journalAllocate);

                }
                foreach (Relation _position in _positions)
                {
                    Journal _journalPosition = new Journal()
                    {
                        RecordID = _pAndLEntity.RecordID,
                        //EntityID = _position.TargetEntity.EntityID,
                        EntityID = _position.Entity.EntityID,
                        BaseCurrency = _position.TargetEntity.Currency.CurrencyID,
                        ExchangeRate = _position.TargetEntity.ExchangeRate,
                        SequenceNo = _pAndLEntity.SequenceNo,
                        //25 = 550 / 0.55 * 0.025
                        //-25 = -550 / 0.55 * 0.025
                        SGDAmount = (_pAndLEntity.SGDAmount / _allocate.Numeric * _position.Numeric).ExtRound(),
                        EntryUser = _pAndLEntity.EntryUser,
                    };

                    _journalPosition.BaseAmount = (_journalPosition.SGDAmount * _journalPosition.ExchangeRate).ExtRound();

                    _result.Add(_journalPosition);
                }
            }

            pAndLEntities.AddRange(_result);

            return pAndLEntities;
        }

        public WeeklySummaryCollection GetWeeklySummary(int entityID) //TODO:
        {
            using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
            {
                HasWeeklySummary(entityID);
                return new WeeklySummaryCollection(_weeklySummaryAccessClient.Query(PeriodService.Instance.GetCurrentPeriod()[0].ID, entityID));
            }
        }

        private void HasWeeklySummary(Entity entity, Dictionary<int, bool> storage)
        {
            WeeklySummaryCollection _weeklySummaryCollection = GetWeeklySummary(entity.EntityID);

            storage[entity.EntityID] = (_weeklySummaryCollection.Count != 0);

            foreach (Entity _entity in entity.SubEntities)
            {
                HasWeeklySummary(_entity, storage);
            }
        }

        private void HasWeeklySummary(Entity entity, Dictionary<Entity, WeeklySummary> storage)
        {
            WeeklySummaryCollection _weeklySummaryCollection = GetWeeklySummary(entity.EntityID);

            storage[entity] = ((_weeklySummaryCollection.Count == 0) ? null : _weeklySummaryCollection[0]);

            foreach (Entity _entity in entity.SubEntities)
            {
                HasWeeklySummary(_entity, storage);
            }
        }

        public Dictionary<int, bool> HasWeeklySummary(Entity entity) //TODO:
        {
            Dictionary<int, bool> _result = new Dictionary<int, bool>();

            HasWeeklySummary(entity, _result);

            return _result;
        }

        private void HasWeeklySummary(int entityID)
        {
            WeeklySummaryCollection _wsa;
            using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
            {
                var _period = PeriodService.Instance.GetCurrentPeriod()[0];
                _wsa = new WeeklySummaryCollection(_weeklySummaryAccessClient.Query(_period.ID, entityID));
                if (!_wsa.Any())
                {
                    using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
                    {
                        var _entity = _entityAccessClient.Query2(entityID)[0];
                        if (_entity.SumType != SumType.Not)
                        {
                            WeeklySummary _newWeeklySummary = new WeeklySummary
                            {
                                Entity = _entity,
                                ExchangeRate = _entity.ExchangeRate,
                                Period = _period,
                                BaseCurrency = _entity.Currency.CurrencyID,
                            };
                            _weeklySummaryAccessClient.Insert1(_newWeeklySummary);
                        }
                    }
                }
                
            }
            
        }

        public Transfer Subtotal(int entryUserID, Entity entity)
        {
            User _user = (new UserAccessClient(EndpointName.UserAccess)).QueryuserID(entryUserID)[0];

            Dictionary<Entity, WeeklySummary> _pair = new Dictionary<Entity, WeeklySummary>();

            HasWeeklySummary(entity, _pair);

            Record _record = RecordHelper.GenerateTempRecord();

            _record.Type = RecordType.WinAndLoss;

            WeeklySummary _weeklySummary = _pair[entity];

            //Transfer
            Transfer _transfer = new Transfer()
            {
                RecordID = _record.RecordID,
                ToEntity = entity,
            };

            if (_weeklySummary == null)
            {
                using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                {
                    _weeklySummaryAccessClient.Insert1(_weeklySummary = new WeeklySummary(PeriodService.Instance.GetCurrentPeriod()[0], entity));
                }
            }

            _transfer.ExchangeRate = _weeklySummary.ExchangeRate;
            _transfer.BaseBefore = _weeklySummary.BaseBalance;
            _transfer.SGDBefore = _weeklySummary.SGDBalance;
            _transfer.Currency.CurrencyID = _weeklySummary.BaseCurrency;
            foreach (Entity _entity in _pair.Keys)
            {
                if (_entity == entity)
                {
                    continue;
                }
                else if ((_weeklySummary = _pair[_entity]) == null)
                {
                    using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                    {
                        _weeklySummaryAccessClient.Insert1(_weeklySummary = new WeeklySummary(PeriodService.Instance.GetCurrentPeriod()[0], _entity));
                    }
                }

                TransferDetail _transferDetail = new TransferDetail()
                {
                    RecordID = _record.RecordID,
                    Entity = _entity,
                    BaseCurrency = _weeklySummary.BaseCurrency,
                    ExchangeRate = _weeklySummary.ExchangeRate,
                    BaseBefore = _weeklySummary.BaseBalance,
                    SGDBefore = _weeklySummary.SGDBalance,
                };

                _transferDetail.BaseTransfer = _transferDetail.BaseBefore;
                _transferDetail.SGDTransfer = _transferDetail.SGDBefore;
                _transferDetail.ProfitAndLoss = 0;

                _transferDetail.BaseResult = 0;
                _transferDetail.SGDResult = 0;

                _transfer.TransferDetailCollection.Add(_transferDetail);
            }

            _transfer.SGDResult = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.SGDBefore).ExtRound();
            _transfer.BaseResult = (_transfer.SGDResult * _transfer.ExchangeRate).ExtRound();

            //Record
            Journal _journal = new Journal()
            {
                RecordID = _record.RecordID,
                EntityID = entity.EntityID,
                BaseCurrency = entity.Currency.CurrencyID,
                ExchangeRate = _transfer.ExchangeRate,
                BaseAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.BaseTransfer),
                SGDAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.SGDTransfer),
                EntryUser = _user,
            };

            _record.JournalCollection.Add(_journal);

            foreach (Entity _entity in _pair.Keys)
            {
                if (_entity == entity)
                {
                    continue;
                }
                else if ((_weeklySummary = _pair[_entity]) == null)
                {
                    continue;
                }

                _journal = new Journal()
                {
                    RecordID = _record.RecordID,
                    EntityID = _entity.EntityID,
                    BaseCurrency = _weeklySummary.BaseCurrency,
                    ExchangeRate = _weeklySummary.ExchangeRate,
                    BaseAmount = _weeklySummary.BaseBalance * -1,
                    SGDAmount = _weeklySummary.SGDBalance * -1,
                    EntryUser = _user,
                };

                _record.JournalCollection.Add(_journal);
            }

            _transfer.RecordNotInDB = _record;

            return _transfer;
        }

        public Tuple<Transfer, Entity> Transfer(int entryUserID, EntityCollection entityCollection, decimal[] baseTransfer)
        {
            User _user = (new UserAccessClient(EndpointName.UserAccess)).QueryuserID(entryUserID)[0];

            Entity _exchangeDiffEntity = EntityService.Instance.LoadEntity(int.Parse(PropertiesService.Instance.GetPropertyValue(SpecialProperty.ExchangeDiff)[0].PropertyValue))[0];

            WeeklySummaryCollection _weeklySummaryCollection = new WeeklySummaryCollection();

            foreach (Entity _entity in entityCollection)
            {
                _weeklySummaryCollection.AddRange(GetWeeklySummary(_entity.EntityID));
            }

            Record _record = RecordHelper.GenerateTempRecord();

            _record.Type = RecordType.Transfer;

            int _index = 0;

            //Transfer
            Transfer _transfer = new Transfer()
            {
                RecordID = _record.RecordID,
                ToEntity = entityCollection[_index],
                Currency = new Currency() { CurrencyID = _weeklySummaryCollection[_index].BaseCurrency },
                ExchangeRate = _weeklySummaryCollection[_index].ExchangeRate,
                BaseBefore = _weeklySummaryCollection[_index].BaseBalance,
                SGDBefore = _weeklySummaryCollection[_index].SGDBalance,
            };

            foreach (Entity _entity in entityCollection)
            {
                if (_index == 0)
                {
                    _index++;

                    continue;
                }

                TransferDetail _transferDetail = new TransferDetail()
                {
                    RecordID = _record.RecordID,
                    Entity = _entity,
                    BaseCurrency = _weeklySummaryCollection[_index].BaseCurrency,
                    ExchangeRate = _weeklySummaryCollection[_index].ExchangeRate,
                    BaseBefore = _weeklySummaryCollection[_index].BaseBalance,
                    SGDBefore = _weeklySummaryCollection[_index].SGDBalance,
                };

                _transferDetail.BaseTransfer = baseTransfer[_index];
                _transferDetail.SGDTransfer = (_transferDetail.BaseTransfer / _transfer.ExchangeRate).ExtRound();
                _transferDetail.ProfitAndLoss = (_transferDetail.BaseTransfer / _transferDetail.ExchangeRate).ExtRound() - _transferDetail.SGDTransfer;

                _transferDetail.BaseResult = 0;
                _transferDetail.SGDResult = 0;

                _transfer.TransferDetailCollection.Add(_transferDetail);

                _index++;
            }

            _transfer.BaseResult = _transfer.TransferDetailCollection.Sum(x=>x.BaseTransfer)+_transfer.BaseBefore;
            _transfer.SGDResult = (_transfer.BaseResult / _transfer.ExchangeRate).ExtRound();

            //Record
            _index = 0;

            Journal _journal = new Journal()
            {
                RecordID = _record.RecordID,
                EntityID = entityCollection[_index].EntityID,
                BaseCurrency = _transfer.Currency.CurrencyID,
                ExchangeRate = _transfer.ExchangeRate,
                BaseAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.BaseTransfer),
                SGDAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.SGDTransfer),
                EntryUser = _user,
            };

            _record.JournalCollection.Add(_journal);

            foreach (Entity _entity in entityCollection)
            {
                if (_index == 0)
                {
                    _index++;

                    continue;
                }

                _journal = new Journal()
                {
                    RecordID = _record.RecordID,
                    EntityID = _entity.EntityID,
                    BaseCurrency = _transfer.TransferDetailCollection[_index - 1].BaseCurrency,
                    ExchangeRate = _transfer.TransferDetailCollection[_index - 1].ExchangeRate,
                    BaseAmount = _transfer.TransferDetailCollection[_index - 1].BaseTransfer * -1,
                    SGDAmount = (_transfer.TransferDetailCollection[_index - 1].SGDTransfer + _transfer.TransferDetailCollection[_index - 1].ProfitAndLoss) * -1,
                    EntryUser = _user,
                };

                _record.JournalCollection.Add(_journal);

                _index++;
            }

            _journal = new Journal()
            {
                RecordID = _record.RecordID,
                EntityID = _exchangeDiffEntity.EntityID,
                SGDAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.ProfitAndLoss)
            };

            _record.JournalCollection.Add(_journal);

            _transfer.RecordNotInDB = _record;

            return new Tuple<Transfer, Entity>(_transfer, _exchangeDiffEntity);
        }

        public Tuple<Transfer, Entity> ExcelTransfer(EntityCollection entityCollection, decimal[] baseTransfer, WeeklySummaryCollection wsc)
        {

            Entity _exchangeDiffEntity = EntityService.Instance.LoadEntity(int.Parse(PropertiesService.Instance.GetPropertyValue(SpecialProperty.ExchangeDiff)[0].PropertyValue))[0];

            //WeeklySummaryCollection _weeklySummaryCollection = new WeeklySummaryCollection();

            //foreach (Entity _entity in entityCollection)
            //{
            //    _weeklySummaryCollection.AddRange(GetWeeklySummary(_entity.EntityID));
            //}

            Record _record = RecordHelper.GenerateTempRecord();

            _record.Type = RecordType.Transfer;

            int _index = 0;

            //Transfer
            Transfer _transfer = new Transfer()
            {
                RecordID = _record.RecordID,
                ToEntity = entityCollection[_index],
                Currency = new Currency() { CurrencyID = wsc[_index].BaseCurrency },
                ExchangeRate = wsc[_index].ExchangeRate,
                BaseBefore = wsc[_index].BaseBalance,
                SGDBefore = wsc[_index].SGDBalance,
            };

            foreach (Entity _entity in entityCollection)
            {
                if (_index == 0)
                {
                    _index++;

                    continue;
                }

                TransferDetail _transferDetail = new TransferDetail()
                {
                    RecordID = _record.RecordID,
                    Entity = _entity,
                    BaseCurrency = wsc[_index].BaseCurrency,
                    ExchangeRate = wsc[_index].ExchangeRate,
                    BaseBefore = wsc[_index].BaseBalance,
                    SGDBefore = wsc[_index].SGDBalance,
                };

                _transferDetail.BaseTransfer = baseTransfer[_index];
                _transferDetail.SGDTransfer = (_transferDetail.BaseTransfer / _transfer.ExchangeRate).ExtRound();
                _transferDetail.ProfitAndLoss = (_transferDetail.BaseTransfer / _transferDetail.ExchangeRate).ExtRound() - _transferDetail.SGDTransfer;

                _transferDetail.BaseResult = 0;
                _transferDetail.SGDResult = 0;

                _transfer.TransferDetailCollection.Add(_transferDetail);

                _index++;
            }

            _transfer.BaseResult = _transfer.TransferDetailCollection.Sum(x => x.BaseTransfer) + _transfer.BaseBefore;
            _transfer.SGDResult = (_transfer.BaseResult / _transfer.ExchangeRate).ExtRound();

            //Record
            _index = 0;

            Journal _journal = new Journal()
            {
                RecordID = _record.RecordID,
                EntityID = entityCollection[_index].EntityID,
                BaseCurrency = _transfer.Currency.CurrencyID,
                ExchangeRate = _transfer.ExchangeRate,
                BaseAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.BaseTransfer),
                SGDAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.SGDTransfer),
            };

            _record.JournalCollection.Add(_journal);

            foreach (Entity _entity in entityCollection)
            {
                if (_index == 0)
                {
                    _index++;

                    continue;
                }

                _journal = new Journal()
                {
                    RecordID = _record.RecordID,
                    EntityID = _entity.EntityID,
                    BaseCurrency = _transfer.TransferDetailCollection[_index - 1].BaseCurrency,
                    ExchangeRate = _transfer.TransferDetailCollection[_index - 1].ExchangeRate,
                    BaseAmount = _transfer.TransferDetailCollection[_index - 1].BaseTransfer * -1,
                    SGDAmount = (_transfer.TransferDetailCollection[_index - 1].SGDTransfer + _transfer.TransferDetailCollection[_index - 1].ProfitAndLoss) * -1,
                };

                _record.JournalCollection.Add(_journal);

                _index++;
            }

            _journal = new Journal()
            {
                RecordID = _record.RecordID,
                EntityID = _exchangeDiffEntity.EntityID,
                SGDAmount = _transfer.TransferDetailCollection.Sum(TransferDetail => TransferDetail.ProfitAndLoss)
            };

            _record.JournalCollection.Add(_journal);

            _transfer.RecordNotInDB = _record;

            return new Tuple<Transfer, Entity>(_transfer, _exchangeDiffEntity);
        }
    }
}
