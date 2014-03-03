using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;

namespace TodoNancy
{
    public class TodosModule : NancyModule
    {
        public TodosModule() : base("todos")
        {
            Get["/"] = _ => Response.AsJson(Store.Values);

            Post["/"] = _ =>
                {
                    var newTodo = this.Bind<Todo>();
                    if (newTodo.Id == 0)
                        newTodo.Id = Store.Count + 1;

                    if (Store.ContainsKey(newTodo.Id))
                        return HttpStatusCode.NotAcceptable;

                    Store.Add(newTodo.Id, newTodo);
                    return Response.AsJson(newTodo).WithStatusCode(HttpStatusCode.Created);
                };

            // p.id refers to the {id} part, it is a dynamic parametercollection. could also be written as p["id"] (same as JavaScript)
            Put["/{id}"] = p =>
                {
                    if (!Store.ContainsKey(p.id))
                        return HttpStatusCode.NotFound;

                    var updated = this.Bind<Todo>();
                    Store[p.Id] = updated;
                    return Response.AsJson(updated);
                };
            Delete["/{id}"] = p =>
                {
                    if (!Store.ContainsKey(p.id))
                        return HttpStatusCode.NotFound;
                    Store.Remove(p.Id);
                    return HttpStatusCode.OK;
                };
        }
        public static Dictionary<long, Todo> Store = new Dictionary<long, Todo>();

    }
}