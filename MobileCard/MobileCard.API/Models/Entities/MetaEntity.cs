using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileCard.API.Models.Entities
{
    public abstract class MetaEntity<TEntity> : MetaEntity<TEntity, string> where TEntity : class
    {
    }

    public abstract class MetaEntity<TEntity, TKey> where TEntity : class where TKey : IEquatable<TKey>
    {
        #region Statics
        static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            Error = (s, e) =>
            {
                Console.Error.WriteLine("An error occured while " +
                    $"attempting to deserialize a resource metadata.\n{e.ErrorContext.Error}");
                e.ErrorContext.Handled = true;
            }
        };
        #endregion

        public virtual string this[string key]
        {
            get => GetMeta()[key];
            set => SetMeta(meta => meta[key] = value);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
        public virtual string Metadata { get; set; }

        [NotMapped]
        protected virtual Dictionary<string, string> Meta { get; set; } = null;


        #region Methods

        public virtual Dictionary<string, string> GetMeta()
        {
            if (Meta != null) return Meta;

            try
            {
                Meta = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(Metadata, SerializerSettings);
            }
            catch
            {
                // Careful, this literally means loss of data.
                // The expectation is that there was no metadata initially 
                // and we are just getting started.
                Meta = new Dictionary<string, string>();
            }

            return Meta;
        }

        public virtual TEntity SetMeta(Action<Dictionary<string, string>> setAction)
        {
            var meta = GetMeta();
            setAction.Invoke(meta);

            Metadata = JsonConvert.SerializeObject(meta, SerializerSettings);
            return this as TEntity;
        }

        #endregion
    }
}
