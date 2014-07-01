using Oleit.AS.Service.DataObject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    public static class RecordHelper
    {
        private static volatile int _recordTempID = 0;
        private static volatile object _recordTempID_SyncRoot = new object();
        private static ConcurrentDictionary<int, Record> _tempRecords = new ConcurrentDictionary<int, Record>();

        public static int RecordTempID
        {
            get
            {
                return _recordTempID;
            }
        }

        public static Record GenerateTempRecord()
        {
            lock (_recordTempID_SyncRoot)
            {
                _recordTempID--;
            }

            _tempRecords[_recordTempID] = new Record()
            {
                RecordID = _recordTempID,
            };

            return _tempRecords[_recordTempID];
        }

        public static Record GetTempRecord(int recordTempID)
        {
            Record _record = null;

            if (_tempRecords.TryGetValue(recordTempID, out _record))
            {
                return _record;
            }

            return null;
        }

        public static Record RemoveTempRecord(int recordTempID)
        {
            Record _record = null;

            if (_tempRecords.TryRemove(recordTempID, out _record))
            {
                return _record;
            }

            return null;
        }
    }
}