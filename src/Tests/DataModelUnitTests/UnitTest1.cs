using System;
using ETWCorrelationViewer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataModelUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ProjectDataConstructorTest()
        {
            ProjectData project1 = new ProjectData(1);
            Assert.AreEqual(1, project1.ID);
            Assert.IsNull(project1.StartPoint);
            Assert.IsNull(project1.EndPoint);
            Assert.AreEqual(0, project1.Data.Count);
        }

        [TestMethod]
        public void BasicNotifyTest()
        {
            ProjectData project1 = new ProjectData(1);
            var startPoint1 = DateTime.Now;
            DataItem item1 = new DataItem(1, startPoint1);
            Assert.AreEqual(startPoint1, item1.StartPoint);
            Assert.IsNull(item1.EndPoint);
            Assert.AreEqual(1, item1.ItemType);

            project1.Data.Add(item1);
            Assert.AreEqual(1, project1.Data.Count);

            Assert.AreEqual(startPoint1, project1.StartPoint);
            Assert.AreEqual(startPoint1, project1.EndPoint);

            var endPoint1 = startPoint1.AddMinutes(5);
            item1.EndPoint = endPoint1;

            Assert.AreEqual(endPoint1, project1.EndPoint);

            project1.Data.Add(new DataItem(1, startPoint1.AddMinutes(2), null));

            var endPoint2 = startPoint1.AddMinutes(10);
            project1.Data.Add(new DataItem(1, startPoint1.AddMinutes(2), endPoint2));

            Assert.AreEqual(endPoint2, project1.EndPoint);
        }

        [TestMethod]
        public void NotifyTestWithSubItems()
        {
            ProjectData project1 = new ProjectData(1);
            var startPoint1 = DateTime.Now;
            var endPoint1 = startPoint1.AddMinutes(5);
            DataItem item1 = new DataItem(1, startPoint1, endPoint1);
            project1.Data.Add(item1);
            Assert.AreEqual(endPoint1, project1.EndPoint);

            var endPoint2 = startPoint1.AddMinutes(10);
            item1.Children.Add(new DataItem(1, startPoint1.AddMinutes(2), endPoint2));

            Assert.AreEqual(endPoint2, project1.EndPoint);
        }

        [TestMethod]
        public void NotifyTestWithIncludedSubItems()
        {
            ProjectData project = new ProjectData(0);
            DateTime now = DateTime.Today;
            project.Data.Add(new DataItem(1, now, now.AddMinutes(5)));
            project.Data.Add(DataItem.Create(2, now.AddMinutes(30), now.AddMinutes(55), DataItem.Create(3, now.AddMinutes(40), now.AddMinutes(45))));

            Assert.AreEqual(now.AddMinutes(30), project.Data[1].StartPoint);
            Assert.AreEqual(now.AddMinutes(55), project.Data[1].EndPoint);

            Assert.AreEqual(now, project.StartPoint);
            Assert.AreEqual(now.AddMinutes(55), project.EndPoint);
        }

        [TestMethod]
        public void AddItemWithNewerStartPointTest()
        {
            ProjectData project = new ProjectData(0);
            DateTime now = DateTime.Today;
            project.Data.Add(new DataItem(1, now, now.AddMinutes(5)));
            project.Data.Add(DataItem.Create(2, now.AddMinutes(30), now.AddMinutes(55), DataItem.Create(3, now.AddMinutes(40), now.AddMinutes(45))));

            Assert.AreEqual(now.AddMinutes(30), project.Data[1].StartPoint);
            Assert.AreEqual(now.AddMinutes(55), project.Data[1].EndPoint);

            Assert.AreEqual(now, project.StartPoint);
            Assert.AreEqual(now.AddMinutes(55), project.EndPoint);

            project.Data.Add(DataItem.Create(1, now.AddMinutes(65)));

            Assert.AreEqual(now.AddMinutes(65), project.EndPoint);
        }

        [TestMethod]
        public void AddSubItemWithNewerStartPointTest()
        {
            ProjectData project = new ProjectData(0);
            DateTime now = DateTime.Today;
            project.Data.Add(DataItem.Create(1, now, now.AddMinutes(5)));

            project.Data.Add(DataItem.Create(2, now.AddMinutes(30), null, DataItem.Create(3, now.AddMinutes(40), now.AddMinutes(45))));

            Assert.AreEqual(now.AddMinutes(30), project.Data[1].StartPoint);
            Assert.AreEqual(now.AddMinutes(45), project.Data[1].EndPoint);

            Assert.AreEqual(now, project.StartPoint);
            Assert.AreEqual(now.AddMinutes(45), project.EndPoint);

            project.Data.Add(DataItem.Create(1, now.AddMinutes(65)));

            Assert.AreEqual(now.AddMinutes(65), project.EndPoint);

            var subItem = DataItem.Create(3, now.AddMinutes(70));

            project.Data[1].Children.Add(subItem);

            Assert.AreEqual(now.AddMinutes(70), project.Data[1].EndPoint);
            Assert.AreEqual(now.AddMinutes(70), project.EndPoint);

            subItem.EndPoint = now.AddMinutes(75);

            Assert.AreEqual(now.AddMinutes(75), project.Data[1].EndPoint);
            Assert.AreEqual(now.AddMinutes(75), project.EndPoint);
        }
    }
}