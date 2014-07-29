﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CacheLibrary.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        private static DataBase<string> GenerateDataBaseOfRandomStringElements(int amountOfElements)
        {
            var generatorRandomStrings = new GeneratorRandomStrings(amountOfElements);
            var dataBase = new DataBase<string>();
            dataBase.InitializeDataBase(generatorRandomStrings.StringDictionary);
            return dataBase;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CacheUnitTest_DecreaseValueOfCacheCapacity_ExceptionThrow()
        {
            var dataBase = new DataBase<string>();
            var cache = new Cache<string>(10, dataBase);
            cache.Capacity = 5;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CacheUnitTest_NegativeValueOfCacheCapacity_ExceptionThrow()
        {
            var dataBase = new DataBase<string>();
            var cache = new Cache<string>(10, dataBase);
            cache.Capacity = -15;
        }

        [TestMethod]
        public void CacheUnitTest_CorrectGettingElementsFromCacheAndDataBase()
        {
            var dataBase = GenerateDataBaseOfRandomStringElements(3);
            var cache = new Cache<string>(2, dataBase);
            var listOfId = dataBase.GetListOfId();
            var elements = new List<Element<string>>();

            foreach (var identifier in listOfId)
                elements.Add(cache.GetElementById(identifier));

            var expectedTypeStorageOfFirstElement = TypesOfStorage.DataBase;
            var expectedTypeStorageOfSecondElement = TypesOfStorage.DataBase;
            var expectedTypeStorageOfThirdElement = TypesOfStorage.DataBase;

            var actualTypeStorageOfFirstElement = elements[0].TypeOfStorage;
            var actualTypeStorageOfSecondElement = elements[1].TypeOfStorage;
            var actualTypeStorageOfThirdElement = elements[2].TypeOfStorage;

            Assert.AreEqual(expectedTypeStorageOfFirstElement, actualTypeStorageOfFirstElement, "First Element From Data Base");
            Assert.AreEqual(expectedTypeStorageOfSecondElement, actualTypeStorageOfSecondElement, "Second Element From Data Base");
            Assert.AreEqual(expectedTypeStorageOfThirdElement, actualTypeStorageOfThirdElement, "Third Element From Data Base");

            RepeatedRequestAndCachingLastTwoElements(cache, listOfId);

            expectedTypeStorageOfFirstElement = TypesOfStorage.DataBase;
            expectedTypeStorageOfSecondElement = TypesOfStorage.Cache;
            expectedTypeStorageOfThirdElement = TypesOfStorage.Cache;

            actualTypeStorageOfFirstElement = elements[0].TypeOfStorage;
            actualTypeStorageOfSecondElement = elements[1].TypeOfStorage;
            actualTypeStorageOfThirdElement = elements[2].TypeOfStorage;

            Assert.AreEqual(expectedTypeStorageOfFirstElement, actualTypeStorageOfFirstElement, "First Element From Data Base");
            Assert.AreEqual(expectedTypeStorageOfSecondElement, actualTypeStorageOfSecondElement, "Second Element From Cache");
            Assert.AreEqual(expectedTypeStorageOfThirdElement, actualTypeStorageOfThirdElement, "Third Element From Cache");
        }

        private static void RepeatedRequestAndCachingLastTwoElements(Cache<string> cache, List<string> listOfId)
        {
            var someInformationAboutSecondElement = cache.GetElementById(listOfId[1]);
            var someInformationAboutThirdElement = cache.GetElementById(listOfId[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CacheUnitTest_NegativeValueOfTimeLimitStoringInCache_ExceptionThrow()
        {
            var dataBase = new DataBase<string>();
            var negativeValueOfTimeLimitStoringInCache = -5;
            var cache = new Cache<string>(10, negativeValueOfTimeLimitStoringInCache, dataBase);
        }
    }
}
