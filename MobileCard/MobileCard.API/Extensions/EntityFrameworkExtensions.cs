using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileCard.API.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static async Task SafelyLoadAsync<TEntity, TProperty>(this
                CollectionEntry<TEntity, TProperty> entry)
            where TEntity : class
            where TProperty : class
        {
            if (!entry.IsLoaded) await entry.LoadAsync();
        }

        public static async Task SafelyLoadAsync<TEntity, TProperty>(this
                ReferenceEntry<TEntity, TProperty> entry)
            where TEntity : class
            where TProperty : class
        {
            if (!entry.IsLoaded) await entry.LoadAsync();
        }

        public static void SafelyLoad<TEntity, TProperty>(this
                CollectionEntry<TEntity, TProperty> entry)
            where TEntity : class
            where TProperty : class
        {
            if (!entry.IsLoaded) entry.Load();
        }

        public static void SafelyLoad<TEntity, TProperty>(this
                ReferenceEntry<TEntity, TProperty> entry)
            where TEntity : class
            where TProperty : class
        {
            if (!entry.IsLoaded) entry.Load();
        }
    }
}
