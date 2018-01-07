namespace EFClassGenerator
{
    public class SQLQuery
    {

        /// <summary>
        /// [EN]    : Search DataTables and Synonyms.
        /// [ESP]   : Query para buscar las Tablas y Sinonimos.
        /// </summary>
        public static string GetTablesAndSynonyms = "SELECT * FROM sys.objects WHERE type in ('SN','U') ORDER BY type DESC";

        /// <summary>
        /// [EN]    : Search Columns from Table.
        /// [ESP]   : Buscar las columnas de las tablas.
        /// 
        /// [EN]:One parameter to replace. / [ESP]: Un parametro a reemplazar.
        /// 
        /// {0}= Table name./ Nombre de la Tabla.
        /// </summary>
        public static string GetTablesByName = "SELECT * FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME='{0}'";
        //      "SELECT * FROM {0}.INFORMATION_SCHEMA.columns WHERE TABLE_NAME='{1}'"


        /// <summary>
        /// [EN]    : Search columns from Synonyms Tables.
        /// [ESP]   : Buscar las columnas de las tabla relacionadas con Sinonimos.
        /// 
        /// [EN]:One parameter to replace. / [ESP]: Un parametro a reemplazar.
        /// 
        /// {0}= Table name./ Nombre de la Tabla.
        /// </summary>
        public static string GetSynonymsByName = "SELECT * FROM sys.synonyms WHERE name='{0}'";

        /// <summary>
        /// 
        /// 
        /// [EN]:Two parameter to replace. / [ESP]: Dos parametros a reemplazar
        /// 
        /// {0}=Path of Table. By default set Empty value.
        /// {1}= Table name./ Nombre de la Tabla.
        /// </summary>
        public static string GetPrimaryKey = @"SELECT KU.table_name as tablename,column_name as primarykeycolumn
                                                    FROM {0}INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                                                    INNER JOIN {0}INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                                                        ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                                                        AND ku.table_name='{1}'
                                                    ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";


        /// <summary>
        /// Get Many to Many relationships 
        /// 
        /// [EN]:Two parameter to replace. / [ESP]: Dos parametros a reemplazar
        /// 
        /// {0}=object_id
        /// {1} = Path db. Set empty by default, or put path.
        /// </summary>
        public static string GetManyToMany = @"
                    DECLARE @object_id int
	                SELECT @object_id = {0}
	
	                --	Declaramos tabla dinamica para guardar la busqueda
	                DECLARE @FKListRelates TABLE (
		                idx smallint Primary Key IDENTITY(1,1)
		                ,name nvarchar(250)
		
		                ,name_parent nvarchar(250)
		                ,id_parent int
		                ,column_parent nvarchar(250)
		
		                ,name_referenced nvarchar(250)
		                ,id_referenced int
		                ,column_referenced nvarchar(250)
	                )
	
	                INSERT @FKListRelates
		                SELECT  
			                fk.name,
			
			                o1.name 'name_parent',
			                fk.parent_object_id 'id_parent',
			                c1.name 'column_parent',
			
			                o2.name 'name_referenced',
			                fk.referenced_object_id 'id_referenced',
			                c2.name 'column_referenced'
			
		                FROM {1}sys.foreign_keys fk
			                INNER JOIN {1}sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
			                INNER JOIN {1}sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
			                INNER JOIN {1}sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
			                INNER JOIN {1}sys.objects o1 ON o1.object_id = c1.object_id
			                INNER JOIN {1}sys.objects o2 ON o2.object_id = c2.object_id
		                WHERE fk.referenced_object_id = @object_id
		
	                DECLARE @i int, 
		                @numrows int,
		                @countColumns int

	                DECLARE @name_parent nvarchar(250)
		                ,@id_parent int
		                ,@column_parent nvarchar(250)
		                ,@name_referenced nvarchar(250)
		                ,@id_referenced int
		                ,@column_referenced nvarchar(250)

	                SET @i = 1
	                SET @numrows = (SELECT COUNT(*) FROM @FKListRelates)
	
	                IF @numrows > 0
		                WHILE (@i <= (SELECT MAX(idx) FROM @FKListRelates))
		                BEGIN
			
			                --	Aca habria q recorer las Tablas que se relacionan y recorer a su ves esas tablas.
			                SELECT @name_parent = l.name_parent
					                ,@id_parent = l.id_parent
					                ,@column_parent = l.column_parent
					                ,@name_referenced = l.name_referenced
					                ,@id_referenced = l.id_referenced
					                ,@column_referenced = l.column_referenced

				                FROM @FKListRelates AS l
				                WHERE idx = @i

				                --	Obtenemos la cantidad de columnas que tiene esa clase padre
				                SELECT @countColumns = count(c.column_id) FROM {1}sys.columns c WHERE c.object_id = @id_parent; 

				                --	Si tiene 2 columnas hay posibilidad de que sea muchos a muchos
				                IF(@countColumns = 2)
					                BEGIN
						                --	Aca faltaria averiguar de traer la otra FK que no sea la de arriba para ver si es M-M
						                SELECT
                                            fk.name AS 'FkName',
                                            fk.parent_object_id 'FkTableId',
	                                        o1.name 'FkTable',
	                                        c1.name 'FkColumn',
                                            fk.referenced_object_id 'PkTableId',
	                                        o2.name 'PkTable',
                                            @column_parent 'PkColumn'
	                                        --c2.name 'PkColumn'

                                            ,CASE 
							                    WHEN fkc.parent_column_id = 2
							                       THEN 1 
							                       ELSE 0 
					                       END as CreateConfig

						                FROM {1}sys.foreign_keys fk
							                INNER JOIN {1}sys.foreign_key_columns fkc 
                                                                ON fkc.constraint_object_id = fk.object_id
                                                                    --AND fkc.parent_column_id = 2
							                INNER JOIN {1}sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
							                INNER JOIN {1}sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
							                INNER JOIN {1}sys.objects o1 ON o1.object_id = c1.object_id
							                INNER JOIN {1}sys.objects o2 ON o2.object_id = c2.object_id
						                WHERE fk.parent_object_id = @id_parent
							                AND fk.referenced_object_id <> @object_id
					                END
								
			                SET @i = @i + 1
		                END";

        /// <summary>
        /// 
        /// [EN]:Two parameter to replace. / [ESP]: Dos parametros a reemplazar
        /// 
        /// {0}=object_id de la tabla
        /// {1} = Path db. Default Vacio
        /// </summary>
        public static string GetOneToMany = @"DECLARE @object_id int
                                                SELECT @object_id = {0}
	
                                                --	Declaramos tabla dinamica para guardar la busqueda
                                                DECLARE @FKListRelates TABLE (
	                                                idx smallint Primary Key IDENTITY(1,1)
	                                                ,name nvarchar(250)
		
	                                                ,name_parent nvarchar(250)
	                                                ,id_parent int
	                                                ,column_parent nvarchar(250)
		
	                                                ,name_referenced nvarchar(250)
	                                                ,id_referenced int
	                                                ,column_referenced nvarchar(250)
                                                )

                                                DECLARE @ReturnListRelates TABLE (
	                                                FkName nvarchar(250)
		
	                                                ,FkTable nvarchar(250)
	                                                ,FkTableId int
	                                                ,FkColumn nvarchar(250)
		
	                                                ,PkTable nvarchar(250)
	                                                ,PkTableId int
	                                                ,PkColumn nvarchar(250)
                                                )
	
                                                INSERT @FKListRelates
	                                                SELECT  
		                                                fk.name,
			
		                                                o1.name 'name_parent',
		                                                fk.parent_object_id 'id_parent',
		                                                c1.name 'column_parent',
			
		                                                o2.name 'name_referenced',
		                                                fk.referenced_object_id 'id_referenced',
		                                                c2.name 'column_referenced'
			
	                                                FROM {1}sys.foreign_keys fk
		                                                INNER JOIN {1}sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
		                                                INNER JOIN {1}sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
		                                                INNER JOIN {1}sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
		                                                INNER JOIN {1}sys.objects o1 ON o1.object_id = c1.object_id
		                                                INNER JOIN {1}sys.objects o2 ON o2.object_id = c2.object_id
	                                                WHERE fk.referenced_object_id = @object_id
		
                                                DECLARE @i int, 
	                                                @numrows int,
	                                                @countColumns int

                                                DECLARE @name_parent nvarchar(250)
	                                                ,@id_parent int
	                                                ,@column_parent nvarchar(250)
	                                                ,@name_referenced nvarchar(250)
	                                                ,@id_referenced int
	                                                ,@column_referenced nvarchar(250)

                                                SET @i = 1
                                                SET @numrows = (SELECT COUNT(*) FROM @FKListRelates)
	
                                                IF @numrows > 0
	                                                WHILE (@i <= (SELECT MAX(idx) FROM @FKListRelates))
	                                                BEGIN
			
		                                                --	Aca habria q recorer las Tablas que se relacionan y recorer a su ves esas tablas.
		                                                SELECT @name_parent = l.name_parent
				                                                ,@id_parent = l.id_parent
				                                                ,@column_parent = l.column_parent
				                                                ,@name_referenced = l.name_referenced
				                                                ,@id_referenced = l.id_referenced
				                                                ,@column_referenced = l.column_referenced

			                                                FROM @FKListRelates AS l
			                                                WHERE idx = @i

			                                                --	Obtenemos la cantidad de columnas que tiene esa clase padre
			                                                SELECT @countColumns = count(c.column_id) FROM {1}sys.columns c WHERE c.object_id = @id_parent; 
			                                                --print( '@name_parent:' + @name_parent + ' - @countColumns: ' + RTRIM( CAST( @countColumns AS nvarchar(30) ) ) ) ;

			                                                --	Si tiene 2 columnas hay posibilidad de que sea muchos a muchos
			                                                IF(@countColumns <> 2)
				                                                BEGIN
					                                                --	Aca faltaria averiguar de traer la otra FK que no sea la de arriba para ver si es M-M
					                                                INSERT @ReturnListRelates
						                                                SELECT
							                                                fk.name AS 'FkName',
							                                                o1.name 'FkTable',
							                                                fk.parent_object_id 'FkTableId',
							                                                c1.name 'FkColumn',

							                                                o2.name 'PkTable',
							                                                fk.referenced_object_id 'PkTableId',
							                                                c2.name 'PkColumn'

							                                                --@column_parent 'PkColumn'

							

						                                                FROM {1}sys.foreign_keys fk
							                                                INNER JOIN {1}sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
							                                                INNER JOIN {1}sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
							                                                INNER JOIN {1}sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
							                                                INNER JOIN {1}sys.objects o1 ON o1.object_id = c1.object_id
							                                                INNER JOIN {1}sys.objects o2 ON o2.object_id = c2.object_id
						                                                WHERE fk.parent_object_id = @id_parent
							                                                AND fk.referenced_object_id = @object_id
				                                                END
								
		                                                SET @i = @i + 1

		
	                                                END
	                                                SELECT * FROM @ReturnListRelates";
    }
}