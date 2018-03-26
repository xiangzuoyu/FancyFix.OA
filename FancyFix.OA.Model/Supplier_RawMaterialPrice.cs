﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Common;
using Dos.ORM;
using Dos.ORM.Common;

namespace FancyFix.OA.Model
{

	/// <summary>
	/// 实体类Supplier_RawMaterialPrice 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("Supplier_RawMaterialPrice")]
	[Serializable]
	public partial class Supplier_RawMaterialPrice : Entity 
	{
		#region Model
		private int _Id;
		private int? _RawMaterialId;
		private int? _Years;
		private decimal? _Month1;
		private decimal? _Month2;
		private decimal? _Month3;
		private decimal? _Month4;
		private decimal? _Month5;
		private decimal? _Month6;
		private decimal? _Month7;
		private decimal? _Month8;
		private decimal? _Month9;
		private decimal? _Month10;
		private decimal? _Month11;
		private decimal? _Month12;
		private DateTime? _AddDate;
		private int? _AddUserId;
		private DateTime? _LastDate;
		private int? _LastUserId;
		private int? _Display;
		private int? _VendorId;
		private int? _PriceFrequency;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			get{ return _Id; }
			set
			{
				this.OnPropertyValueChange(_.Id,_Id,value);
				this._Id=value;
			}
		}
		/// <summary>
		/// 原材料ID
		/// </summary>
		public int? RawMaterialId
		{
			get{ return _RawMaterialId; }
			set
			{
				this.OnPropertyValueChange(_.RawMaterialId,_RawMaterialId,value);
				this._RawMaterialId=value;
			}
		}
		/// <summary>
		/// 年份
		/// </summary>
		public int? Years
		{
			get{ return _Years; }
			set
			{
				this.OnPropertyValueChange(_.Years,_Years,value);
				this._Years=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month1
		{
			get{ return _Month1; }
			set
			{
				this.OnPropertyValueChange(_.Month1,_Month1,value);
				this._Month1=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month2
		{
			get{ return _Month2; }
			set
			{
				this.OnPropertyValueChange(_.Month2,_Month2,value);
				this._Month2=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month3
		{
			get{ return _Month3; }
			set
			{
				this.OnPropertyValueChange(_.Month3,_Month3,value);
				this._Month3=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month4
		{
			get{ return _Month4; }
			set
			{
				this.OnPropertyValueChange(_.Month4,_Month4,value);
				this._Month4=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month5
		{
			get{ return _Month5; }
			set
			{
				this.OnPropertyValueChange(_.Month5,_Month5,value);
				this._Month5=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month6
		{
			get{ return _Month6; }
			set
			{
				this.OnPropertyValueChange(_.Month6,_Month6,value);
				this._Month6=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month7
		{
			get{ return _Month7; }
			set
			{
				this.OnPropertyValueChange(_.Month7,_Month7,value);
				this._Month7=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month8
		{
			get{ return _Month8; }
			set
			{
				this.OnPropertyValueChange(_.Month8,_Month8,value);
				this._Month8=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month9
		{
			get{ return _Month9; }
			set
			{
				this.OnPropertyValueChange(_.Month9,_Month9,value);
				this._Month9=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month10
		{
			get{ return _Month10; }
			set
			{
				this.OnPropertyValueChange(_.Month10,_Month10,value);
				this._Month10=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month11
		{
			get{ return _Month11; }
			set
			{
				this.OnPropertyValueChange(_.Month11,_Month11,value);
				this._Month11=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Month12
		{
			get{ return _Month12; }
			set
			{
				this.OnPropertyValueChange(_.Month12,_Month12,value);
				this._Month12=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddDate
		{
			get{ return _AddDate; }
			set
			{
				this.OnPropertyValueChange(_.AddDate,_AddDate,value);
				this._AddDate=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AddUserId
		{
			get{ return _AddUserId; }
			set
			{
				this.OnPropertyValueChange(_.AddUserId,_AddUserId,value);
				this._AddUserId=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastDate
		{
			get{ return _LastDate; }
			set
			{
				this.OnPropertyValueChange(_.LastDate,_LastDate,value);
				this._LastDate=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastUserId
		{
			get{ return _LastUserId; }
			set
			{
				this.OnPropertyValueChange(_.LastUserId,_LastUserId,value);
				this._LastUserId=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Display
		{
			get{ return _Display; }
			set
			{
				this.OnPropertyValueChange(_.Display,_Display,value);
				this._Display=value;
			}
		}
		/// <summary>
		/// 供应商ID
		/// </summary>
		public int? VendorId
		{
			get{ return _VendorId; }
			set
			{
				this.OnPropertyValueChange(_.VendorId,_VendorId,value);
				this._VendorId=value;
			}
		}
		/// <summary>
		/// 价格频次
		/// </summary>
		public int? PriceFrequency
		{
			get{ return _PriceFrequency; }
			set
			{
				this.OnPropertyValueChange(_.PriceFrequency,_PriceFrequency,value);
				this._PriceFrequency=value;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// 获取实体中的标识列
		/// </summary>
		public override Field GetIdentityField()
		{
			return _.Id;
		}
		/// <summary>
		/// 获取实体中的主键列
		/// </summary>
		public override Field[] GetPrimaryKeyFields()
		{
			return new Field[] {
				_.Id};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.Id,
				_.RawMaterialId,
				_.Years,
				_.Month1,
				_.Month2,
				_.Month3,
				_.Month4,
				_.Month5,
				_.Month6,
				_.Month7,
				_.Month8,
				_.Month9,
				_.Month10,
				_.Month11,
				_.Month12,
				_.AddDate,
				_.AddUserId,
				_.LastDate,
				_.LastUserId,
				_.Display,
				_.VendorId,
				_.PriceFrequency};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._Id,
				this._RawMaterialId,
				this._Years,
				this._Month1,
				this._Month2,
				this._Month3,
				this._Month4,
				this._Month5,
				this._Month6,
				this._Month7,
				this._Month8,
				this._Month9,
				this._Month10,
				this._Month11,
				this._Month12,
				this._AddDate,
				this._AddUserId,
				this._LastDate,
				this._LastUserId,
				this._Display,
				this._VendorId,
				this._PriceFrequency};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// * 
			/// </summary>
			public readonly static Field All = new Field("*","Supplier_RawMaterialPrice");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Id = new Field("Id","Supplier_RawMaterialPrice","Id");
			/// <summary>
			/// 原材料ID
			/// </summary>
			public readonly static Field RawMaterialId = new Field("RawMaterialId","Supplier_RawMaterialPrice","原材料ID");
			/// <summary>
			/// 年份
			/// </summary>
			public readonly static Field Years = new Field("Years","Supplier_RawMaterialPrice","年份");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month1 = new Field("Month1","Supplier_RawMaterialPrice","Month1");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month2 = new Field("Month2","Supplier_RawMaterialPrice","Month2");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month3 = new Field("Month3","Supplier_RawMaterialPrice","Month3");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month4 = new Field("Month4","Supplier_RawMaterialPrice","Month4");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month5 = new Field("Month5","Supplier_RawMaterialPrice","Month5");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month6 = new Field("Month6","Supplier_RawMaterialPrice","Month6");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month7 = new Field("Month7","Supplier_RawMaterialPrice","Month7");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month8 = new Field("Month8","Supplier_RawMaterialPrice","Month8");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month9 = new Field("Month9","Supplier_RawMaterialPrice","Month9");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month10 = new Field("Month10","Supplier_RawMaterialPrice","Month10");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month11 = new Field("Month11","Supplier_RawMaterialPrice","Month11");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Month12 = new Field("Month12","Supplier_RawMaterialPrice","Month12");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field AddDate = new Field("AddDate","Supplier_RawMaterialPrice","AddDate");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field AddUserId = new Field("AddUserId","Supplier_RawMaterialPrice","AddUserId");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastDate = new Field("LastDate","Supplier_RawMaterialPrice","LastDate");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastUserId = new Field("LastUserId","Supplier_RawMaterialPrice","LastUserId");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Display = new Field("Display","Supplier_RawMaterialPrice","Display");
			/// <summary>
			/// 供应商ID
			/// </summary>
			public readonly static Field VendorId = new Field("VendorId","Supplier_RawMaterialPrice","供应商ID");
			/// <summary>
			/// 价格频次
			/// </summary>
			public readonly static Field PriceFrequency = new Field("PriceFrequency","Supplier_RawMaterialPrice","价格频次");
		}
		#endregion


	}
}

