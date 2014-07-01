using System;
using System.Collections.Generic;
using System.Linq;
using Oleit.AS.Service.DataObject;
using Accounting_System.EntityServiceReference;


namespace Accounting_System
{
    public class EntitiesFunc
    {

        public static EntityType entityTypeFunc(int entityType)
        {
            EntityType _entityType = entityType == 1 ? EntityType.PAndL :
                    entityType == 2 ? EntityType.Cash :
                    entityType == 3 ? EntityType.Expence :
                    entityType == 4 ? EntityType.BadDebt : EntityType.MLJ;
            return _entityType;
        }

        public static EntityType entityTypeFunc(string entityType)
        {
            EntityType _entityType = entityType.Equals("PAndL", StringComparison.OrdinalIgnoreCase) ? EntityType.PAndL :
                    entityType.Equals("Cash", StringComparison.OrdinalIgnoreCase) ? EntityType.Cash :
                    entityType.Equals("Expence", StringComparison.OrdinalIgnoreCase) ? EntityType.Expence :
                    entityType.Equals("BadDebt", StringComparison.OrdinalIgnoreCase) ? EntityType.BadDebt : EntityType.MLJ;
            return _entityType;
        }

        public static AccountType accoutnTypeFunc(int accountType)
        {
            AccountType _accountType = accountType == 1 ? AccountType.SuperSenior :
                    accountType == 2 ? AccountType.Senior :
                    accountType == 3 ? AccountType.Master :
                    accountType == 4 ? AccountType.Agent : AccountType.Members;
            return _accountType;
        }

        public static SumType sumTypeFunc(int sumType)
        {
            SumType _sumType;
            #region "sumType swith"
            switch (sumType)
            {
                case 0:
                    _sumType = SumType.Not;
                    break;
                case 1:
                    _sumType = SumType.Transaction;
                    break;
                case 2:
                    _sumType = SumType.Subtotal;
                    break;
                case 3:
                    _sumType = SumType.Super;
                    break;
                case 4:
                    _sumType = SumType.Master;
                    break;
                case 5:
                    _sumType = SumType.Agent;
                    break;
                default:
                    _sumType = SumType.Not;
                    break;
            }
            #endregion
            return _sumType; 
        }

        public static void entityInsert(Entity entity)
        {
            var _esc = new EntityServiceClient();
            _esc.NewEntity1(entity);
        }

        public static void entityInsert(Entity entity, CashEntity cashEntity)
        {
            var _esc = new EntityServiceClient();
            _esc.NewEntity2(entity, cashEntity);
        }

        public static void entityInsert(Entity entity, Account account)
        {
            var _esc = new EntityServiceClient();
            var _parentEntity = _esc.LoadEntity2(entity.ParentID)[0];
            entity.ExchangeRate = _parentEntity.ExchangeRate;
            entity.Currency.CurrencyID = _parentEntity.Currency.CurrencyID;
                _esc.NewEntity3(entity, account);
        }

        public static void entityUpdate(Entity entity)
        {
            var _esc = new EntityServiceClient();
            _esc.SaveEntity1(entity);
        }

        public static void entityUpdate(Entity entity, CashEntity cashEntity)
        {
            var _esc = new EntityServiceClient();
            _esc.SaveEntity2(entity, cashEntity);
        }

        public static void entityUpdate(Entity entity, Account account)
        {
            var _esc = new EntityServiceClient();
            _esc.SaveEntity3(SessionData.UserID,entity, account);
        }

        public static EntityCollection entityCollectioin(EntityCollection ec, EntityCollection entityCollection)
        {
            foreach (var entity in entityCollection)
            {
                ec.Add(entity);
                if (entity.SubEntities.Count() > 0)
                    entityCollectioin(ec, entity.SubEntities);
            }
            return ec;
        }
    }


}