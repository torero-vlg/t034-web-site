﻿using System;
using System.Collections.Generic;
using AutoMapper;
using T034.Core.DataAccess;
using T034.Core.Dto.Common;
using T034.Core.Profiles;

namespace T034.Core.Services.Common
{
    /// <summary>
    /// Основные операции справочника
    /// </summary>
    public abstract class AbstractRepository<TEntity, TDto, TId> 
        where TEntity : Entity.Entity 
        where TDto : AbstractDto<TId>, new()
    {
        protected IBaseDb Db { get; set; }

        public AbstractRepository(IBaseDb db)
        {
            Db = db;
        }

        public TDto Create(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            var result = Db.SaveOrUpdate(entity);
            return Mapper.Map<TDto>(entity);
        }

        public TDto Update(TDto dto)
        {
            var entity = Db.Get<TEntity>(dto.Id);
            entity = Mapper.Map<TEntity>(dto);

            var result = Db.SaveOrUpdate(entity);

            return Mapper.Map<TDto>(entity);
        }

        public IEnumerable<TDto> Select()
        {
            var list = new List<TDto>();
            var items = Db.Select<TEntity>();
            list = Mapper.Map(items, list);
            return list;
        }

        public TDto Get(object id)
        {
            var dto = new TDto();
            var item = Db.Get<TEntity>(id);
            dto = Mapper.Map(item, dto);
            return dto;
        }

        public OperationResult Delete(object id)
        {
            try
            {
                var entity = Db.Get<TEntity>(id);
                var result = Db.Delete(entity);
                if(result)
                    return new OperationResult { Status = StatusOperation.Success };
                return new OperationResult { Status = StatusOperation.InternalError, Message = "Удаление завершилось с ошибкой" };
            }
            catch (Exception)
            {
                return new OperationResult { Status = StatusOperation.InternalError, Message = "Произошла внутренняя ошибка" };
            }
        }

        /// <summary>
        /// Маппер
        /// </summary>
        protected IMapper Mapper => AutoMapperConfig.Mapper;
    }
}
