using Csrs.Interfaces.Dynamics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csrs.Interfaces.Dynamics.Extensions
{
    public static class EntityDocumentExtensions
    {

        public static string GetDocumentFolderName(this MicrosoftDynamicsCRMssgCsrsfile entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            string idCleaned = CleanGuidForSharePoint(entity.SsgCsrsfileid);
            string folderName = $"{entity.SsgFilenumber}_{idCleaned}";
            return folderName;
        }

        public static string GetDocumentFolderName(this MicrosoftDynamicsCRMssgCsrscommunicationmessage entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            // TODO

            //string idCleaned = CleanGuidForSharePoint(entity.SsgCsrscommunicationmessageid);
            //string folderName = $"{entity.SsgFilenumber}_{idCleaned}";
            //return folderName;

            return null;
        }

        private static string CleanGuidForSharePoint(string guidString)
        {
            string result = null;
            if (guidString != null)
            {
                result = guidString.ToUpper().Replace("-", "");
            }
            return result;
        }
    }
}
