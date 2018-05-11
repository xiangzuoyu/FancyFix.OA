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
	/// 实体类Product_Files 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("Product_Files")]
	[Serializable]
	public partial class Product_Files : Entity 
	{
		#region Model
		private int _Id;
		private string _FilePath;
		private string _FileExt;
		private int _FileSize;
		private string _Md5;
		private string _FileName;
		private DateTime? _AddTime;
		private int? _ProId;
		private string _Tag;
		/// <summary>
		/// 自增Id
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
		/// 图片路径
		/// </summary>
		public string FilePath
		{
			get{ return _FilePath; }
			set
			{
				this.OnPropertyValueChange(_.FilePath,_FilePath,value);
				this._FilePath=value;
			}
		}
		/// <summary>
		/// 图片格式
		/// </summary>
		public string FileExt
		{
			get{ return _FileExt; }
			set
			{
				this.OnPropertyValueChange(_.FileExt,_FileExt,value);
				this._FileExt=value;
			}
		}
		/// <summary>
		/// 文件大小
		/// </summary>
		public int FileSize
		{
			get{ return _FileSize; }
			set
			{
				this.OnPropertyValueChange(_.FileSize,_FileSize,value);
				this._FileSize=value;
			}
		}
		/// <summary>
		/// 文件MD5
		/// </summary>
		public string Md5
		{
			get{ return _Md5; }
			set
			{
				this.OnPropertyValueChange(_.Md5,_Md5,value);
				this._Md5=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FileName
		{
			get{ return _FileName; }
			set
			{
				this.OnPropertyValueChange(_.FileName,_FileName,value);
				this._FileName=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AddTime
		{
			get{ return _AddTime; }
			set
			{
				this.OnPropertyValueChange(_.AddTime,_AddTime,value);
				this._AddTime=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ProId
		{
			get{ return _ProId; }
			set
			{
				this.OnPropertyValueChange(_.ProId,_ProId,value);
				this._ProId=value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tag
		{
			get{ return _Tag; }
			set
			{
				this.OnPropertyValueChange(_.Tag,_Tag,value);
				this._Tag=value;
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
				_.FilePath,
				_.FileExt,
				_.FileSize,
				_.Md5,
				_.FileName,
				_.AddTime,
				_.ProId,
				_.Tag};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._Id,
				this._FilePath,
				this._FileExt,
				this._FileSize,
				this._Md5,
				this._FileName,
				this._AddTime,
				this._ProId,
				this._Tag};
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
			public readonly static Field All = new Field("*","Product_Files");
			/// <summary>
			/// 自增Id
			/// </summary>
			public readonly static Field Id = new Field("Id","Product_Files","自增Id");
			/// <summary>
			/// 图片路径
			/// </summary>
			public readonly static Field FilePath = new Field("FilePath","Product_Files","图片路径");
			/// <summary>
			/// 图片格式
			/// </summary>
			public readonly static Field FileExt = new Field("FileExt","Product_Files","图片格式");
			/// <summary>
			/// 文件大小
			/// </summary>
			public readonly static Field FileSize = new Field("FileSize","Product_Files","文件大小");
			/// <summary>
			/// 文件MD5
			/// </summary>
			public readonly static Field Md5 = new Field("Md5","Product_Files","文件MD5");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field FileName = new Field("FileName","Product_Files","FileName");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field AddTime = new Field("AddTime","Product_Files","AddTime");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ProId = new Field("ProId","Product_Files","ProId");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Tag = new Field("Tag","Product_Files","Tag");
		}
		#endregion


	}
}

