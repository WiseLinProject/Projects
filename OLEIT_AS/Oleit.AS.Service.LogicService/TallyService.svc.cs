using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.RecordAccessReference;
using Oleit.AS.Service.LogicService.UserAccessReference;
using Oleit.AS.Service.LogicService.WeeklySummaryReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TallyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TallyService.svc or TallyService.svc.cs at the Solution Explorer and start debugging.
    public class TallyService : ITallyService
    {
        public static volatile TallyService Instance = new TallyService();

        public void DoWork()
        {
        }

        private EntityCollection SetLastLevel(EntityCollection entityCollection, WeeklySummaryCollection weeklySummaryCollection)
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                if (entityCollection[i].IsLastLevel == 1)
                {
                    WeeklySummaryCollection _weeklySummaryCollection = CalculateService.Instance.GetWeeklySummary(entityCollection[i].EntityID);

                    if (_weeklySummaryCollection.Count == 0)
                    {
                        entityCollection.RemoveAt(i--);
                    }
                    else
                    {
                        weeklySummaryCollection.AddRange(_weeklySummaryCollection);
                    }

                    continue;
                }

                EntityCollection _subEntities = entityCollection[i].SubEntities;

                entityCollection.RemoveAt(i);

                if (_subEntities.Count > 0)
                {
                    entityCollection.InsertRange(i, _subEntities);
                }

                i--;
            }

            return entityCollection;
        }

        private EntityCollection SetSubtotal(EntityCollection entityCollection, WeeklySummaryCollection weeklySummaryCollection)
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                if (entityCollection[i].SumType == SumType.Subtotal)
                {
                    entityCollection[i].SubEntities = SetLastLevel(entityCollection[i].SubEntities, weeklySummaryCollection);

                    WeeklySummaryCollection _weeklySummaryCollection = CalculateService.Instance.GetWeeklySummary(entityCollection[i].EntityID);

                    if (_weeklySummaryCollection.Count == 0)
                    {
                        WeeklySummary _weeklySummary = new WeeklySummary(PeriodService.Instance.GetCurrentPeriod()[0], entityCollection[i]);

                        using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                        {
                            _weeklySummaryAccessClient.Insert1(_weeklySummary);
                        }

                        weeklySummaryCollection.Add(_weeklySummary);
                    }
                    else
                    {
                        weeklySummaryCollection.AddRange(_weeklySummaryCollection);
                    }

                    continue;
                }

                EntityCollection _subEntities = entityCollection[i].SubEntities;

                entityCollection.RemoveAt(i);

                if (_subEntities.Count > 0)
                {
                    entityCollection.InsertRange(i, _subEntities);
                }

                i--;
            }

            return entityCollection;
        }

        private EntityCollection SetTransaction(EntityCollection entityCollection, WeeklySummaryCollection weeklySummaryCollection)
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                if (entityCollection[i].SumType == SumType.Transaction)
                {
                    entityCollection[i].SubEntities = SetSubtotal(entityCollection[i].SubEntities, weeklySummaryCollection);

                    WeeklySummaryCollection _weeklySummaryCollection = CalculateService.Instance.GetWeeklySummary(entityCollection[i].EntityID);

                    if (_weeklySummaryCollection.Count == 0)
                    {
                        WeeklySummary _weeklySummary = new WeeklySummary(PeriodService.Instance.GetCurrentPeriod()[0], entityCollection[i]);

                        using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                        {
                            _weeklySummaryAccessClient.Insert1(_weeklySummary);
                        }

                        weeklySummaryCollection.Add(_weeklySummary);
                    }
                    else
                    {
                        weeklySummaryCollection.AddRange(_weeklySummaryCollection);
                    }

                    continue;
                }

                EntityCollection _subEntities = entityCollection[i].SubEntities;

                entityCollection.RemoveAt(i);

                if (_subEntities.Count > 0)
                {
                    entityCollection.InsertRange(i, _subEntities);
                }

                i--;
            }

            return entityCollection;
        }

        private WeeklySummaryCollection ConfirmTransfer(EntityCollection entityCollection, WeeklySummaryCollection forSave)
        {
            WeeklySummaryCollection _return = new WeeklySummaryCollection();

            foreach (Entity _entity in entityCollection)
            {
                if (_entity.IsLastLevel == 1)
                {
                    WeeklySummaryCollection _lastLevel = CalculateService.Instance.GetWeeklySummary(_entity.EntityID);

                    if (_lastLevel.Count == 0)
                    {
                        continue;
                    }

                    _lastLevel.All(lastLevel =>
                    {
                        List<decimal> _winAndLoss = DataEntryService.Instance.GetjournalSum(PeriodService.Instance.GetCurrentPeriod()[0].ID, (int)SumType.Subtotal, lastLevel.Entity.EntityID);

                        if (_winAndLoss.Count < 2)
                        {
                            return true;
                        }

                        lastLevel.BaseWinAndLoss = _winAndLoss[0];
                        lastLevel.SGDWinAndLoss = _winAndLoss[1];

                        return true;
                    });

                    forSave.AddRange(_lastLevel);
                    _return.AddRange(_lastLevel);
                }
                else if (_entity.SumType == SumType.Subtotal)
                {
                    WeeklySummaryCollection _lastLevel = ConfirmTransfer(_entity.SubEntities, forSave);

                    WeeklySummary _subtotal = CalculateService.Instance.GetWeeklySummary(_entity.EntityID)[0];

                    _subtotal.BaseWinAndLoss = _lastLevel.Sum(WeeklySummary => WeeklySummary.BaseWinAndLoss);
                    _subtotal.SGDWinAndLoss = _lastLevel.Sum(WeeklySummary => WeeklySummary.SGDWinAndLoss);

                    List<decimal> _transfer = DataEntryService.Instance.GetjournalSum(PeriodService.Instance.GetCurrentPeriod()[0].ID, (int)SumType.Transaction, _subtotal.Entity.EntityID);

                    if (_transfer.Count >= 2)
                    {
                        _subtotal.BaseTransfer = _transfer[0];
                        _subtotal.SGDTransfer = _transfer[1];
                    }

                    _lastLevel.All(lastLevel =>
                        {
                            lastLevel.BaseBalance = lastLevel.BasePrevBalance + lastLevel.BaseWinAndLoss + lastLevel.BaseTransfer;
                            lastLevel.SGDBalance = lastLevel.SGDPrevBalance + lastLevel.SGDWinAndLoss + lastLevel.SGDTransfer;

                            return true;
                        });

                    _subtotal.BaseBalance = _subtotal.BasePrevBalance + _subtotal.BaseWinAndLoss + _subtotal.BaseTransfer;
                    _subtotal.SGDBalance = _subtotal.SGDPrevBalance + _subtotal.SGDWinAndLoss + _subtotal.SGDTransfer;

                    forSave.Add(_subtotal);
                    _return.Add(_subtotal);
                }
                else if (_entity.SumType == SumType.Transaction)
                {
                    WeeklySummaryCollection _subtotal = ConfirmTransfer(_entity.SubEntities, forSave);

                    WeeklySummary _transfer = CalculateService.Instance.GetWeeklySummary(_entity.EntityID)[0];

                    _transfer.BaseWinAndLoss = _subtotal.Sum(WeeklySummary => WeeklySummary.BaseBalance);
                    _transfer.SGDWinAndLoss = _subtotal.Sum(WeeklySummary => WeeklySummary.SGDBalance);

                    _subtotal.All(subtotal =>
                    {
                        //subtotal.BaseBalance = subtotal.BasePrevBalance + subtotal.BaseWinAndLoss + subtotal.BaseTransfer;
                        //subtotal.SGDBalance = subtotal.SGDPrevBalance + subtotal.SGDWinAndLoss + subtotal.SGDTransfer;

                        return true;
                    });

                    _transfer.BaseBalance = _transfer.BasePrevBalance + _transfer.BaseWinAndLoss + _transfer.BaseTransfer;
                    _transfer.SGDBalance = _transfer.SGDPrevBalance + _transfer.SGDWinAndLoss + _transfer.SGDTransfer;

                    forSave.Add(_transfer);
                    _return.Add(_transfer);
                }

                continue;
            }

            return _return;
        }

        private WeeklySummaryCollection ExcelConfirmTransfer(EntityCollection entityCollection, WeeklySummaryCollection forSave)
        {
            WeeklySummaryCollection _return = new WeeklySummaryCollection();

            foreach (Entity _entity in entityCollection)
            {
                if (_entity.IsLastLevel == 1)
                {
                    WeeklySummaryCollection _lastLevel = forSave;

                    if (_lastLevel.Count == 0)
                    {
                        continue;
                    }

                    forSave.AddRange(_lastLevel);
                    _return.AddRange(_lastLevel);
                }
                else if (_entity.SumType == SumType.Subtotal)
                {
                    WeeklySummaryCollection _lastLevel = ConfirmTransfer(_entity.SubEntities, forSave);

                    WeeklySummary _subtotal = CalculateService.Instance.GetWeeklySummary(_entity.EntityID)[0];

                    if (_subtotal.Status == WeeklySummaryStatus.Confirm)
                    {
                        forSave.Add(_subtotal);
                        _return.Add(_subtotal);

                        continue;
                    }

                    _subtotal.BaseTransfer = _lastLevel.Sum(WeeklySummary => WeeklySummary.BaseWinAndLoss);
                    _subtotal.SGDTransfer = _lastLevel.Sum(WeeklySummary => WeeklySummary.SGDWinAndLoss);

                    _lastLevel.All(lastLevel =>
                    {
                        lastLevel.BaseTransfer = lastLevel.BaseWinAndLoss * -1;
                        lastLevel.SGDTransfer = lastLevel.SGDWinAndLoss * -1;

                        lastLevel.BaseBalance += lastLevel.BasePrevBalance + lastLevel.BaseWinAndLoss + lastLevel.BaseTransfer;
                        lastLevel.SGDBalance += lastLevel.SGDPrevBalance + lastLevel.SGDWinAndLoss + lastLevel.SGDTransfer;

                        return true;
                    });

                    _subtotal.BaseBalance += _subtotal.BasePrevBalance + _subtotal.BaseWinAndLoss + _subtotal.BaseTransfer;
                    _subtotal.SGDBalance += _subtotal.SGDPrevBalance + _subtotal.SGDWinAndLoss + _subtotal.SGDTransfer;

                    forSave.Add(_subtotal);
                    _return.Add(_subtotal);
                }
                else if (_entity.SumType == SumType.Transaction)
                {
                    WeeklySummaryCollection _subtotal = ConfirmTransfer(_entity.SubEntities, forSave);

                    WeeklySummary _transfer = CalculateService.Instance.GetWeeklySummary(_entity.EntityID)[0];

                    if (_transfer.Status == WeeklySummaryStatus.Confirm)
                    {
                        forSave.Add(_transfer);
                        _return.Add(_transfer);

                        continue;
                    }

                    _transfer.BaseTransfer = _subtotal.Sum(WeeklySummary => WeeklySummary.BaseTransfer);
                    _transfer.SGDTransfer = _subtotal.Sum(WeeklySummary => WeeklySummary.SGDTransfer);

                    _subtotal.All(subtotal =>
                    {
                        subtotal.BaseBalance += subtotal.BasePrevBalance + subtotal.BaseWinAndLoss + subtotal.BaseTransfer;
                        subtotal.SGDBalance += subtotal.SGDPrevBalance + subtotal.SGDWinAndLoss + subtotal.SGDTransfer;

                        return true;
                    });

                    _transfer.BaseBalance += _transfer.BasePrevBalance + _transfer.BaseWinAndLoss + _transfer.BaseTransfer;
                    _transfer.SGDBalance += _transfer.SGDPrevBalance + _transfer.SGDWinAndLoss + _transfer.SGDTransfer;

                    forSave.Add(_transfer);
                    _return.Add(_transfer);
                }

                continue;
            }

            return _return;
        }

        public Tuple<EntityCollection, WeeklySummaryCollection> LoadEntity(int entityID)
        {
            EntityCollection _entityTree = EntityService.Instance.LoadEntity();
            WeeklySummaryCollection _weeklySummaryCollection = new WeeklySummaryCollection();

            _entityTree.RemoveAll(Entity => ((Entity.EntityID != entityID) && (Entity.ParentID == 0)));

            foreach (Entity _entity in _entityTree)
            {
                _entity.SubEntities = SetTransaction(_entity.SubEntities, _weeklySummaryCollection);

                WeeklySummaryCollection _transferButNotSave = new WeeklySummaryCollection();

                ConfirmTransfer(_entity.SubEntities, _transferButNotSave);

                for (int i = 0; i < _weeklySummaryCollection.Count; i++)
                {
                    WeeklySummary _transfer = _transferButNotSave.FirstOrDefault(WeeklySummary => (WeeklySummary.Entity.EntityID == _weeklySummaryCollection[i].Entity.EntityID));

                    if (_transfer == null)
                    {
                        continue;
                    }

                    _weeklySummaryCollection[i] = _transfer;
                }
            }

            return new Tuple<EntityCollection, WeeklySummaryCollection>(_entityTree, _weeklySummaryCollection);
        }

        public Tuple<EntityCollection, WeeklySummaryCollection> LoadPAndLEntity(int entityID)
        {
            EntityCollection _entityTree = EntityService.Instance.LoadEntity();
            WeeklySummaryCollection _weeklySummaryCollection = new WeeklySummaryCollection();

            _entityTree.RemoveAll(Entity => ((Entity.EntityID != entityID) && (Entity.ParentID == 0)));

            foreach (Entity _entity in _entityTree)
            {
                _entity.SubEntities = SetTransaction(_entity.SubEntities, _weeklySummaryCollection);

                WeeklySummaryCollection _transferButNotSave = new WeeklySummaryCollection();

                ConfirmTransfer(_entity.SubEntities, _transferButNotSave);

                for (int i = 0; i < _weeklySummaryCollection.Count; i++)
                {
                    WeeklySummary _transfer = _transferButNotSave.FirstOrDefault(WeeklySummary => (WeeklySummary.Entity.EntityID == _weeklySummaryCollection[i].Entity.EntityID));

                    if (_transfer == null)
                    {
                        continue;
                    }

                    _weeklySummaryCollection[i] = _transfer;
                }
            }

            return new Tuple<EntityCollection, WeeklySummaryCollection>(_entityTree, _weeklySummaryCollection);
        }

        public WeeklySummaryCollection Confirm(int confirmUserID, int entityID)
        {
            User _confirmUser = (new UserAccessClient(EndpointName.UserAccess)).QueryuserID(confirmUserID)[0];

            WeeklySummaryCollection _confirm = new WeeklySummaryCollection();

            Entity _entity = EntityService.Instance.LoadEntity(entityID)[0];

            if ((_entity.SumType != SumType.Transaction) && (_entity.SumType != SumType.Subtotal))
            {
                return _confirm;
            }

            _entity.SubEntities = ((_entity.SumType == SumType.Transaction) ? SetSubtotal(_entity.SubEntities, new WeeklySummaryCollection()) : SetLastLevel(_entity.SubEntities, new WeeklySummaryCollection()));

            ConfirmTransfer(new EntityCollection(new Entity[] { _entity }), _confirm);

            if (_confirm.Count <= 0)
            {
                return _confirm;
            }

            foreach (WeeklySummary _weeklySummary in _confirm)
            {
                _weeklySummary.Status = WeeklySummaryStatus.Confirm;
                _weeklySummary.ConfirmUser = _confirmUser;
            }

            using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
            {
                _weeklySummaryAccessClient.Update2(_confirm.ToArray());
            }

            //<EntityID, RecordID>
            Dictionary<int, int> _pairs = new Dictionary<int, int>();

            foreach (WeeklySummary _weeklySummary in _confirm)
            {
                int _recordID = -1;

                using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    _pairs[_weeklySummary.Entity.EntityID] = (_recordID = _recordAccessClient.QueryRecordID(_weeklySummary.Entity.EntityID, PeriodService.Instance.GetCurrentPeriod()[0].ID));
                }

                if (_recordID > 0)
                {
                    if (_pairs.Values.Count(RecordID => (RecordID == _recordID)) == 1)
                    {
                        using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                        {
                            _recordAccessClient.ChangeStatus(_recordID, RecordStatus.Confirm);
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, int> _pair in _pairs)
            {
                if (_pair.Value > 0)
                {
                    continue;
                }

                Entity _subtotal = null;

                if (_entity.SumType == SumType.Transaction)
                {
                    if ((_subtotal = _entity.SubEntities.FirstOrDefault(Entity => (_pair.Key == Entity.EntityID))) == null)
                    {
                        continue;
                    }
                }
                else if (_pair.Key == _entity.EntityID)
                {
                    _subtotal = _entity;
                }
                else
                {
                    continue;
                }

                EntityCollection _entityCollection = new EntityCollection() { _subtotal };

                _entityCollection.AddRange(_subtotal.SubEntities);

                decimal[] _baseTransfer = new decimal[_entityCollection.Count];

                for (int i = 1; i < _entityCollection.Count; i++)
                {
                    _baseTransfer[i] = _confirm.FirstOrDefault(WeeklySummary => (WeeklySummary.Entity.EntityID == _entityCollection[i].EntityID)).BaseTransfer;
                }

                Transfer _transfer = CalculateService.Instance.Transfer(confirmUserID, _entityCollection, _baseTransfer).Item1;

                DataEntryService.Instance.InsertTransfer(_transfer.RecordNotInDB, _transfer);
            }

            return _confirm;
        }

        public WeeklySummaryCollection ExcelConfirm(int entityID, WeeklySummaryCollection wsc)
        {

            Entity _entity = EntityService.Instance.LoadEntity(entityID)[0];

            if ((_entity.SumType != SumType.Transaction) && (_entity.SumType != SumType.Subtotal))
            {
                return wsc;
            }

            _entity.SubEntities = ((_entity.SumType == SumType.Transaction) ? SetSubtotal(_entity.SubEntities, new WeeklySummaryCollection()) : SetLastLevel(_entity.SubEntities, new WeeklySummaryCollection()));

            ExcelConfirmTransfer(new EntityCollection(new Entity[] { _entity }), wsc);

            if (wsc.Count <= 0)
            {
                return wsc;
            }

            //foreach (WeeklySummary _weeklySummary in _confirm)
            //{
            //    _weeklySummary.Status = WeeklySummaryStatus.Confirm;
            //    _weeklySummary.ConfirmUser = _confirmUser;
            //}

            //using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
            //{
            //    _weeklySummaryAccessClient.Update2(_confirm.ToArray());
            //}

            //<EntityID, RecordID>
            Dictionary<int, int> _pairs = new Dictionary<int, int>();

            foreach (WeeklySummary _weeklySummary in wsc)
            {
                int _recordID = -1;

                using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                {
                    _pairs[_weeklySummary.Entity.EntityID] = (_recordID = _recordAccessClient.QueryRecordID(_weeklySummary.Entity.EntityID, PeriodService.Instance.GetCurrentPeriod()[0].ID));
                }

                if (_recordID > 0)
                {
                    if (_pairs.Values.Count(RecordID => (RecordID == _recordID)) == 1)
                    {
                        using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
                        {
                            _recordAccessClient.ChangeStatus(_recordID, RecordStatus.Confirm);
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, int> _pair in _pairs)
            {
                if (_pair.Value > 0)
                {
                    continue;
                }

                Entity _subtotal = null;

                if (_entity.SumType == SumType.Transaction)
                {
                    if ((_subtotal = _entity.SubEntities.FirstOrDefault(Entity => (_pair.Key == Entity.EntityID))) == null)
                    {
                        continue;
                    }
                }
                else if (_pair.Key == _entity.EntityID)
                {
                    _subtotal = _entity;
                }
                else
                {
                    continue;
                }

                EntityCollection _entityCollection = new EntityCollection() { _subtotal };

                _entityCollection.AddRange(_subtotal.SubEntities);

                decimal[] _baseTransfer = new decimal[_entityCollection.Count];

                for (int i = 1; i < _entityCollection.Count; i++)
                {
                    _baseTransfer[i] = wsc.FirstOrDefault(WeeklySummary => (WeeklySummary.Entity.EntityID == _entityCollection[i].EntityID)).BaseTransfer;
                }

                Transfer _transfer = CalculateService.Instance.ExcelTransfer(_entityCollection, _baseTransfer, wsc).Item1;

                DataEntryService.Instance.InsertTransfer(_transfer.RecordNotInDB, _transfer);
            }

            return wsc;
        }
    }
}
