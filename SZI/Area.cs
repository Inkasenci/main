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
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    public partial class Area : IItem
    {
        public System.Guid AreaId { get; set; }
        public string Street { get; set; }
        public string CollectorId { get; set; }

        public string[] GetElements
        {
            get
            {
                return new string[]
                {
                    AreaId.ToString(),
                    Street,
                    CollectorId
                };
            }
        }

        public void InsertIntoDB()
        {
            try
            {
                using (var database = new CollectorsManagementSystemEntities())
                {
                    database.Areas.Add(this);
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
                    "UPDATE Area SET Street={0}, CollectorId={1} WHERE AreaId={2}",
                    this.Street,
                    this.CollectorId,
                    id
                );
                dataBase.SaveChanges();
            }
        }
    }
}