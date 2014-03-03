using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Testing;
using Nancy;
using Xunit;
using TodoNancy;

namespace TodoNancyTests
{
    public class TodosModuleTests
    {
        private Browser _sut;
        private Todo _aTodo;
        private Todo _anEditedTodo;

        public TodosModuleTests()
        {
            TodosModule.Store.Clear();
            _sut = new Browser(new DefaultNancyBootstrapper());

            _aTodo = new Todo
            {
                Title = "task 1", Order = 0, Completed = false
            };

            _anEditedTodo = new Todo
            {
                Id = 42,
                Title = "edited name",
                Order = 0,
                Completed = false
            };
        }

        [Fact]
        public void Should_return_empty_list_on_get_when_no_todos_have_been_posted()
        {
            var actual = _sut.Get("/todos/");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.Empty(actual.Body.DeserializeJson<Todo[]>());
        }

        [Fact]
        public void Should_return_201_create_when_a_todo_is_posted()
        {
            var actual = _sut.Post("/todos/", with => 
                with.JsonBody(_aTodo) );

            Assert.Equal(HttpStatusCode.Created, actual.StatusCode);
        }

        [Fact]
        public void Should_not_accept_posting_to_with_duplicate_id  ()
        {
            var actual = _sut.Post("/todos/", with =>
                with.JsonBody(_anEditedTodo))
                .Then
                .Post("/todos/", with =>
                with.JsonBody(_anEditedTodo));

            Assert.Equal(HttpStatusCode.NotAcceptable, actual.StatusCode);
        }

        [Fact]
        public void Should_be_able_to_get_posted_todo()
        {
            var actual = _sut.Post("/todos/", with =>
                with.JsonBody(_aTodo))
                .Then
                .Get("/todos/");

            var actualBody = actual.Body.DeserializeJson<Todo[]>();
            Assert.Equal(1, actualBody.Length);
            AssertAreSame(_aTodo, actualBody[0]);
        }

        [Fact]
        public void Should_be_able_to_edit_todo_with_put()
        {
            var actual = _sut.Post("/todos/", with =>
                with.JsonBody(_aTodo))
                .Then
                .Put("/todos/1", with =>
                with.JsonBody(_anEditedTodo))
                .Then
                .Get("/todos/");

            var actualBody = actual.Body.DeserializeJson<Todo[]>();
            Assert.Equal(1, actualBody.Length);
            AssertAreSame(_anEditedTodo, actualBody[0]);
        }

        [Fact]
        public void Should_be_able_to_delete_todo_with_delete()
        {
            var actual = _sut.Post("/todos/", with =>
                with.JsonBody(_aTodo))
                .Then
                .Delete("/todos/1")
                .Then
                .Get("/todos/");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.Empty(actual.Body.DeserializeJson<Todo[]>());
        }

        private void AssertAreSame(Todo expected, Todo actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Order, actual.Order);
            Assert.Equal(expected.Completed, actual.Completed);
        }
    }
}
