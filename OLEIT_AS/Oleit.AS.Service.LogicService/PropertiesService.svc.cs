using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.PropertyAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PropertiesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PropertiesService.svc or PropertiesService.svc.cs at the Solution Explorer and start debugging.
    public class PropertiesService : IPropertiesService
    {
        public static volatile PropertiesService Instance = new PropertiesService();

        public void DoWork()
        {
        }

        public void NewProperty(Property property)
        {
            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                _propertyAccessClient.Insert1(property);
            }
        }

        public void NewProperty(PropertyCollection propertyCollection)
        {
            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                _propertyAccessClient.Insert2(propertyCollection.ToArray());
            }
        }
        /// <summary>
        /// Don't use this class
        /// </summary>     
        public void SetPropertyValue(int propertyID, string propertyValue)
        {
            //PropertyCollection _propertyCollection = GetPropertyValue(propertyID);

            //foreach (Property _property in _propertyCollection)
            //{
            //    _property.PropertyValue = propertyValue;
            //}

            //using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            //{
            //    _propertyAccessClient.Update2(_propertyCollection.ToArray());
            //}
        }
        
        public void SetPropertyValue(string propertyName, string propertyValue)
        {
            PropertyCollection _propertyCollection = GetPropertyValue(propertyName);

            foreach (Property _property in _propertyCollection)
            {
                _property.PropertyValue = propertyValue;
            }

            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                _propertyAccessClient.Update2(_propertyCollection.ToArray());
            }
        }

        private static volatile PropertyCollection ClosedPeriodCache = new PropertyCollection();

        /// <summary>
        /// Please use this function to update the property instead of PropertyAccessClient.Update1.
        /// This is very important, or the system may has error.
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="perty"></param>
        public void SetProperty(string propertyKey ,Property perty)
        {            
            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                _propertyAccessClient.Update1(propertyKey, perty);
            }

            if (propertyKey.Equals(SpecialProperty.ClosedPeriod))
            {
                ClosedPeriodCache = GetPropertyValue(propertyKey);
            }
        }

        /// <summary>
        /// Don't use this class
        /// </summary>
        public PropertyCollection GetPropertyValue(int propertyID)
        {
            //using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            //{
            //    return new PropertyCollection(_propertyAccessClient.Query1(propertyID));
            //}
            return null;
        }

        public PropertyCollection GetPropertyValue(string propertyName)
        {
            if (propertyName.Equals(SpecialProperty.ClosedPeriod))
            {
                if (ClosedPeriodCache.Count == 0)
                {
                    using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
                    {
                        ClosedPeriodCache = new PropertyCollection(_propertyAccessClient.Query2(propertyName));
                    }
                }

                return ClosedPeriodCache;
            }

            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                return new PropertyCollection(_propertyAccessClient.Query2(propertyName));
            }
        }

        public PropertyCollection GetProperty(string propertyName)
        {
            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                return new PropertyCollection(_propertyAccessClient.Query2(propertyName));
            }
        }

        public PropertyCollection GetAllProperties()
        {
            using (PropertyAccessClient _propertyAccessClient = new PropertyAccessClient(EndpointName.PropertyAccess))
            {
                return new PropertyCollection(_propertyAccessClient.QueryAll());
            }
        }
    }
}
