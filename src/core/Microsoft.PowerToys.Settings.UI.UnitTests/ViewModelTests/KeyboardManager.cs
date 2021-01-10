﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerToys.Settings.UI.Lib;
using Microsoft.PowerToys.Settings.UI.Lib.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ViewModelTests
{
    [TestClass]
    public class KeyboardManager
    {
        public const string Module = "Keyboard Manager";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void CombineShortcutLists_ShouldReturnEmptyList_WhenBothArgumentsAreEmptyLists()
        {
            // arrange
            var firstList = new List<KeysDataModel>();
            var secondList = new List<AppSpecificKeysDataModel>();

            // act
            var result = KeyboardManagerViewModel.CombineShortcutLists(firstList, secondList);

            // Assert
            var expectedResult = new List<AppSpecificKeysDataModel>();

            Assert.AreEqual(expectedResult.Count(), result.Count());
        }

        [TestMethod]
        public void CombineShortcutLists_ShouldReturnListWithOneAllAppsEntry_WhenFirstArgumentHasOneEntryAndSecondArgumentIsEmpty()
        {
            // arrange
            var firstList = new List<KeysDataModel>();
            var entry = new KeysDataModel();
            entry.OriginalKeys = "17;65";
            entry.NewRemapKeys = "17;86";
            firstList.Add(entry);
            var secondList = new List<AppSpecificKeysDataModel>();

            // act
            var result = KeyboardManagerViewModel.CombineShortcutLists(firstList, secondList);

            // Assert
            var expectedResult = new List<AppSpecificKeysDataModel>();
            var expectedEntry = new AppSpecificKeysDataModel();
            expectedEntry.OriginalKeys = entry.OriginalKeys;
            expectedEntry.NewRemapKeys = entry.NewRemapKeys;
            expectedEntry.TargetApp = "All Apps";
            expectedResult.Add(expectedEntry);
            var x = expectedResult[0].Equals(result[0]);
            Assert.AreEqual(expectedResult.Count(), result.Count());
            Assert.IsTrue(expectedResult[0].Compare(result[0]));
        }

        [TestMethod]
        public void CombineShortcutLists_ShouldReturnListWithOneAppSpecificEntry_WhenFirstArgumentIsEmptyAndSecondArgumentHasOneEntry()
        {
            // arrange
            var firstList = new List<KeysDataModel>();
            var secondList = new List<AppSpecificKeysDataModel>();
            var entry = new AppSpecificKeysDataModel();
            entry.OriginalKeys = "17;65";
            entry.NewRemapKeys = "17;86";
            entry.TargetApp = "msedge";
            secondList.Add(entry);

            // act
            var result = KeyboardManagerViewModel.CombineShortcutLists(firstList, secondList);

            // Assert
            var expectedResult = new List<AppSpecificKeysDataModel>();
            var expectedEntry = new AppSpecificKeysDataModel();
            expectedEntry.OriginalKeys = entry.OriginalKeys;
            expectedEntry.NewRemapKeys = entry.NewRemapKeys;
            expectedEntry.TargetApp = entry.TargetApp;
            expectedResult.Add(expectedEntry);

            Assert.AreEqual(expectedResult.Count(), result.Count());
            Assert.IsTrue(expectedResult[0].Compare(result[0]));
        }

        [TestMethod]
        public void CombineShortcutLists_ShouldReturnListWithOneAllAppsEntryAndOneAppSpecificEntry_WhenFirstArgumentHasOneEntryAndSecondArgumentHasOneEntry()
        {
            // arrange
            var firstList = new List<KeysDataModel>();
            var firstListEntry = new KeysDataModel();
            firstListEntry.OriginalKeys = "17;65";
            firstListEntry.NewRemapKeys = "17;86";
            firstList.Add(firstListEntry);
            var secondList = new List<AppSpecificKeysDataModel>();
            var secondListEntry = new AppSpecificKeysDataModel();
            secondListEntry.OriginalKeys = "17;66";
            secondListEntry.NewRemapKeys = "17;87";
            secondListEntry.TargetApp = "msedge";
            secondList.Add(secondListEntry);

            // act
            var result = KeyboardManagerViewModel.CombineShortcutLists(firstList, secondList);

            // Assert
            var expectedResult = new List<AppSpecificKeysDataModel>();
            var expectedFirstEntry = new AppSpecificKeysDataModel();
            expectedFirstEntry.OriginalKeys = firstListEntry.OriginalKeys;
            expectedFirstEntry.NewRemapKeys = firstListEntry.NewRemapKeys;
            expectedFirstEntry.TargetApp = "All Apps";
            expectedResult.Add(expectedFirstEntry);
            var expectedSecondEntry = new AppSpecificKeysDataModel();
            expectedSecondEntry.OriginalKeys = secondListEntry.OriginalKeys;
            expectedSecondEntry.NewRemapKeys = secondListEntry.NewRemapKeys;
            expectedSecondEntry.TargetApp = secondListEntry.TargetApp;
            expectedResult.Add(expectedSecondEntry);

            Assert.AreEqual(expectedResult.Count(), result.Count());
            Assert.IsTrue(expectedResult[0].Compare(result[0]));
            Assert.IsTrue(expectedResult[1].Compare(result[1]));
        }
    }
}
