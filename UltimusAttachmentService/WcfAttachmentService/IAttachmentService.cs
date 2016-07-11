using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace WcfAttachmentService
{
    [ServiceContract]
    public interface IAttachmentService
    {
        [OperationContract]
        bool UploadFile(string processName, string incident, string step, string fileName, byte[] fileData);

        [OperationContract]
        string[] GetFileList(string processName, string incident, string step);

        [OperationContract]
        byte[] GetFile(string processName, string incident, string step, string fileName);

        [OperationContract]
        bool DeleteFile(string processName, string incident, string step, string fileName);

        [OperationContract]
        bool MoveFiles(string processName, string sourceIncident, string targetIncident);

        [OperationContract]
        bool DeleteFiles(string processName, string tmpDirectory);
    }
}
