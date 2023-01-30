using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Manager
{
	public enum AuditEventTypes
	{
		AuthenticationSuccess = 0,
		AuthorizationSuccess = 1,
		AuthorizationFailure = 2,
		PunishStudent = 3,
		ForgiveStudent = 4
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string AuthenticationSuccess
		{
			get
			{
                // TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString()); 
			}
		}

		public static string AuthorizationSuccess
		{
			get
			{
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
		}

		public static string AuthorizationFailed
		{
			get
			{
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailure.ToString());
            }
		}

		public static string PunishStudent
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.PunishStudent.ToString());
			}
		}

        public static string ForgiveStudent
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ForgiveStudent.ToString());
            }
        }
    }
}
