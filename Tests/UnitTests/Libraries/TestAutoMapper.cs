﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer.EfClasses;
using Tests.Dtos;
using Tests.Helpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Libraries
{
    public class TestAutoMapper
    {
        [Fact]
        public void TestDirectMappingBookTitle()
        {
            //SETUP
            var mapper = AutoMapperHelpers.CreateMapper<Book, BookTitle>();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().First();
            var data = mapper.Map<BookTitle>(input);

            //VERIFY
            data.Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestProjectionMappingBookTitle()
        {
            //SETUP
            var mapper = AutoMapperHelpers.CreateMapper<Book, BookTitle>();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().AsQueryable();
            var list = input.ProjectTo<BookTitle>(mapper.ConfigurationProvider).ToList();

            //VERIFY
            list.First().Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestDirectMappingBookTitleAndCount()
        {
            //SETUP
            var mapper = AutoMapperHelpers.CreateMapper<Book, BookTitleAndCount>();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().First();
            var data = mapper.Map<BookTitleAndCount>(input);

            //VERIFY
            data.Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestProjectionMappingBookTitleAndCount()
        {
            //SETUP
            var mapper = BookTitleAndCount.Config.CreateMapper();

            //ATTEMPT
            var input = DddEfTestData.CreateFourBooks().AsQueryable();
            var list = input.ProjectTo<BookTitleAndCount>(mapper.ConfigurationProvider).ToList();

            //VERIFY
            list.Last().Title.ShouldEqual("Quantum Networking");
            list.Last().ReviewsCount.ShouldEqual(2);
        }

        [Fact]
        public void TestProjectionMappingBookTitleBadType()
        {
            //SETUP
            var mapper = AutoMapperHelpers.CreateMapper<Book, BookTitleBadType>();

            //ATTEMPT
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            //VERIFY
            //Doesn't error on name fit but different 
        }

        [Fact]
        public void TestDirectMappingToBookNotSetPrivateSetter()
        {
            //SETUP
            var mapper = AutoMapperHelpers.CreateMapper<BookTitle, Book>();
            var entity = DddEfTestData.CreateFourBooks().First();

            //ATTEMPT
            var dto = new BookTitle {Title = "New Title"};
            var data = mapper.Map(dto, entity);

            //VERIFY
            data.Title.ShouldEqual("Refactoring");
        }

        [Fact]
        public void TestPacalNamingConvention()
        {
            //SETUP
            var pascal = new PascalCaseNamingConvention();

            //ATTEMPT
            var result = pascal.SplittingExpression.Replace("ThisIsPascal", "$1 ");

            //VERIFY
            result.ShouldEqual("This Is Pascal ");
        }
    }
}