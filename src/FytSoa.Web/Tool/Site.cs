using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FytSoa.Application.Cms;
using FytSoa.Common;
using FytSoa.Common.Utils;
using FytSoa.Domain.Cms;
using FytSoa.Common.Cache;

namespace FytSoa.Web
{
    public class Site
    {
        private static CmsSiteDto _site;
        /// <summary>
        /// 当前用户对象
        /// </summary>
        public static CmsSiteDto CurrentSite
        {
            get
            {
               
                var webSite = MemoryService.Default.GetCache<CmsSiteDto>(KeyUtils.WEBCMSSITE);
                if (webSite!=null && webSite.Id!=0)
                {
                    _site = webSite;
                }
                else
                {
                    var siteService = AppUtils.GetService<CmsSiteService>();
                    _site = siteService.GetAsync(1364837482360868864).Result;
                    MemoryService.Default.SetCache(key: KeyUtils.WEBCMSSITE, _site);
                }
                return _site;
            }
        }
    }
}
