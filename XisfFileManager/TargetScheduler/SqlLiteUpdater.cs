using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfFileManager.TargetScheduler
{
    internal class SqlLiteUpdater
    {
        private SqlLiteManager mSqlManager;

        public SqlLiteUpdater(SqlLiteManager manager)
        {
            mSqlManager = manager;
        }
    }
}
