/***********************************************************************
 * Module:  Admin.cs
 * Author:  Administrator
 * Purpose: Definition of the Class Admin
 ***********************************************************************/

using System;
using System.Runtime.Serialization;

namespace Model
{
    /// <summary>
    /// �Ĳ�ʵ����
    /// </summary>
    [DataContract]
    public class Consumable
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ConsumableCategory { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SuppierName { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string Specification { get; set; }
        [DataMember]
        public string Unit { get; set; }
        [DataMember]
        public string SupplierNumber { get; set; }
        [DataMember]
        public string DeliveryDay { get; set; }
        [DataMember]
        public int Warehousing { get; set; }
        [DataMember]
        public int Delivery { get; set; }
        [DataMember]
        public int Stock { get; set; }      // ���
        [DataMember]
        public int MinStock { get; set; }      // ��С���
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Description { get; set; }


        [DataMember]
        public int Creater { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int Modifier { get; set; }
        [DataMember]
        public DateTime ModifiedDate { get; set; }
    }

    [DataContract]
    public class ConsumableWrap
    {
        [DataMember]
        public Consumable Consumable { get; set; }
    }
}