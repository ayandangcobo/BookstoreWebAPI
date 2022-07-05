using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace BookstoreWebAPI
{
    public class AddFileParamTypes : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "Books_AddBook")
            {
                operation.consumes.Add("multipart/form-data");
                var paramList = new List<Parameter>
                    {
                        new Parameter
                        {
                            type = "file",
                            name = "book_cover",
                            description = "Book Cover",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "title",
                            description = "Title",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "description",
                            description = "Description",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "integer",
                            description = "Author User Id",
                            name = "author",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "number",
                            name = "price",
                            description = "Price",
                            required = false,
                            multipleOf = 2,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            name = "Authorization",
                            @in = "header",
                            description = "access token",
                            required = false,
                            type = "string"
                        }
                    };
                operation.parameters = paramList;

            }
        }
    }


}