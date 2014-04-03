
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SZI
{

    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    
    public partial class Address
    {
        public System.Guid AddressId { get; set; }
        public int HouseNo { get; set; }
        public Nullable<int> FlatNo { get; set; }
        public System.Guid AreaId { get; set; }

        public string[] GetElements
        {
            get
            {
                return new string[]
                {
                    AddressId.ToString(),
                    HouseNo.ToString(),
                    FlatNo.ToString(),
                    AreaId.ToString()
                };
            }
        }

        public void InsertIntoDB()
        {
            try
            {
                using (var database = new CollectorsManagementSystemEntities())
                {
                    database.Addresses.Add(this);
                    database.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                var innerEx = ex.InnerException;

                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;

                System.Windows.Forms.MessageBox.Show(innerEx.Message);
            }
        }

        public void ModifyRecord(string id)
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                dataBase.Database.ExecuteSqlCommand
                (
                    "UPDATE Address SET HouseNo={0}, FlatNo={1}, AreaId={2} WHERE AddressId={3}",
                    this.HouseNo,
                    this.FlatNo,
                    this.AreaId,
                    id
                );
                dataBase.SaveChanges();
            }
        }
    }

}
