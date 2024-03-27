using EducationTech.Business.Models.Master;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Abstract
{
    public abstract class Model : IModel
    {
        [NotMapped]
        [JsonIgnore]
        public virtual bool SoftDelete => false;

        [NotMapped]
        [JsonIgnore]
        public virtual bool Timestamp => false;

        [JsonIgnore]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonIgnore]
        public DateTimeOffset? DeletedAt { get; set; }
        public abstract void OnModelCreating(ModelBuilder modelBuilder);


        protected void ConfigureSideEffects<TModel>(ModelBuilder modelBuilder) where TModel : Model
        {

            if (typeof(TModel).IsAbstract || typeof(TModel).IsInterface || !typeof(TModel).IsAssignableTo(typeof(Model)))
            {
                throw new Exception("TModel must be a class that implements Model");
            }
            var model = modelBuilder.Entity<TModel>();
            if (!Timestamp)
            {
                model
                    .Ignore(nameof(CreatedAt))
                    .Ignore(nameof(UpdatedAt));
            }

            if (!SoftDelete)
            {
                model.Ignore(nameof(DeletedAt));
            }
            else
            {
                model.HasQueryFilter(c => c.DeletedAt == null);
            }
        }

        public void Map<TDto>(TDto dto) 
            where TDto : class 
        {
            //use reflection to map properties has the same name in dto to model
            var modelProperties = GetType().GetProperties();
            var dtoProperties = dto.GetType().GetProperties();
            var dtoPropertiesDict = dtoProperties.ToDictionary(prop => prop.Name, prop => prop);

            foreach (var modelProperty in modelProperties)
            {
                if(!dtoPropertiesDict.ContainsKey(modelProperty.Name))
                {
                    continue;
                }
                var dtoProperty = dtoPropertiesDict[modelProperty.Name];
                if(dtoProperty.PropertyType == modelProperty.PropertyType)
                {
                    var value = dtoProperty.GetValue(dto);
                    if(value != null)
                    {
                        modelProperty.SetValue(this, dtoProperty.GetValue(dto));
                    }
                }
            }
        }
    }
}
