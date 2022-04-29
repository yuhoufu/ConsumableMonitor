/***********************************************************************
 * Module:  Admin.cs
 * Author:  Administrator
 * Purpose: Definition of the Class Admin
 ***********************************************************************/

using System;
using System.Runtime.Serialization;

namespace ConsumableMonitor
{
    /// <summary>
    /// ember元数据实体类
    /// </summary>
    [DataContract]
    public class Meta
    {
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public int Page { get; set; }
        [DataMember]
        public int Total { get; set; }


        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string Msg { get; set; }


    }

}