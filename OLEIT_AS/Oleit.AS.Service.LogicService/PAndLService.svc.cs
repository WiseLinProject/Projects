using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.EntityAccessReference;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PAndLService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PAndLService.svc or PAndLService.svc.cs at the Solution Explorer and start debugging.
    public class PAndLService : IPAndLService
    {
        public static volatile PAndLService Instance = new PAndLService();

        public void DoWork()
        {
        }

        private EntityCollection GetTransaction(EntityCollection entityCollection)
        {
            for (int i = 0; i < entityCollection.Count; i++)
            {
                if (entityCollection[i].SumType == SumType.Transaction)
                {
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

        private void GetWeeklySummary(EntityCollection entityCollection, WeeklySummaryCollection weeklySummaryCollection)
        {
            foreach (Entity _entity in entityCollection)
            {
                GetWeeklySummary(_entity.SubEntities, weeklySummaryCollection);

                WeeklySummaryCollection _weeklySummaryCollection = CalculateService.Instance.GetWeeklySummary(_entity.EntityID);

                if (_weeklySummaryCollection.Count == 0)
                {
                    WeeklySummary _weeklySummary = new WeeklySummary(PeriodService.Instance.GetClosedPeriod()[0], _entity);

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
            }
        }

        public void SetPAndL(int confirmUserID)
        {
            User _confirmUser = (new UserAccessClient(EndpointName.UserAccess)).QueryuserID(confirmUserID)[0];
            Period _closedPeriod = PeriodService.Instance.GetClosedPeriod()[0];

            RecordCollection _recordCollection = null;

            using (RecordAccessClient _recordAccessClient = new RecordAccessClient(EndpointName.RecordAccess))
            {
                _recordCollection = new RecordCollection(_recordAccessClient.QueryByperiod(_closedPeriod));
            }

            foreach (Record _record in _recordCollection)
            {
                if (_record.RecordStatus != RecordStatus.Confirm)
                {
                    throw new ArgumentException(string.Format("[if (_record.RecordStatus != RecordStatus.Confirm)][{0}][{1}][{2}]", _record.RecordID, _record.Type, _record.RecordStatus));
                }
            }

            EntityCollection _entityTree = EntityService.Instance.LoadEntity();
            EntityCollection _pAndL = null;

            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _pAndL = new EntityCollection(_entityAccessClient.Query4(EntityType.PAndL));
            }

            for (int i = 0; i < _pAndL.Count; i++)
            {
                _pAndL[i] = EntityService.Instance.FindEntity(_pAndL[i].EntityID, _entityTree);
            }

            //<PAndL, Transaction>
            Dictionary<Entity, EntityCollection> _transactions = new Dictionary<Entity, EntityCollection>();
            //<PAndL, WeeklySummaryCollection>
            Dictionary<Entity, WeeklySummaryCollection> _weeklySummaryCollection = new Dictionary<Entity, WeeklySummaryCollection>();

            foreach (Entity _entity in _pAndL)
            {
                _transactions[_entity] = GetTransaction(_entity.SubEntities);

                _weeklySummaryCollection[_entity] = new WeeklySummaryCollection();

                GetWeeklySummary(_transactions[_entity], _weeklySummaryCollection[_entity]);

                foreach (WeeklySummary _weeklySummary in _weeklySummaryCollection[_entity])
                {
                    _weeklySummary.Status = WeeklySummaryStatus.Confirm;
                    _weeklySummary.ConfirmUser = _confirmUser;
                }

                using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                {
                    _weeklySummaryAccessClient.Update2(_weeklySummaryCollection[_entity].ToArray());
                }

                Entity _targetEntity = EntityService.Instance.GetRelateEntity(_entity.EntityID)[0].TargetEntity;

                WeeklySummaryCollection _targetWeeklySummary = CalculateService.Instance.GetWeeklySummary(_targetEntity.EntityID);

                if (_targetWeeklySummary.Count == 0)
                {
                    WeeklySummary _weeklySummary = new WeeklySummary(PeriodService.Instance.GetClosedPeriod()[0], _targetEntity);

                    using (WeeklySummaryAccessClient _weeklySummaryAccessClient = new WeeklySummaryAccessClient(EndpointName.WeeklySummaryAccess))
                    {
                        _weeklySummaryAccessClient.Insert1(_weeklySummary);
                    }

                    _targetWeeklySummary.Add(_weeklySummary);
                }

                _targetWeeklySummary.AddRange(_weeklySummaryCollection[_entity]);

                EntityCollection _entityCollection = new EntityCollection() { _targetEntity };

                _entityCollection.AddRange(Array.ConvertAll(_weeklySummaryCollection[_entity].ToArray(), (WeeklySummary => WeeklySummary.Entity)));

                Transfer _transfer = CalculateService.Instance.Transfer(
                    confirmUserID,
                    _entityCollection, Array.ConvertAll(_targetWeeklySummary.ToArray(), (WeeklySummary => WeeklySummary.BaseTransfer))
                    ).Item1;

                DataEntryService.Instance.InsertTransfer(_transfer.RecordNotInDB, _transfer);
            }
        }
    }
}
