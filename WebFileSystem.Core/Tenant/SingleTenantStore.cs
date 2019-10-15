using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFileSystem.Tenant {
    public class SingleTenantStore : ITenantStore {
        public const int SingleTenantId = 1;
        public const string SingleTenancyName = "Default";
        public static TenantInfo SingleTenantInfo = new TenantInfo(SingleTenantId, SingleTenancyName);
        public SingleTenantStore() { }
        public TenantInfo Find(int tenantId)
        {
            return tenantId == SingleTenantId ? SingleTenantInfo : null;
        }
        public TenantInfo Find(string tenancyName)
        {
            return tenancyName == SingleTenancyName ? SingleTenantInfo : null;
        }
    }
}
