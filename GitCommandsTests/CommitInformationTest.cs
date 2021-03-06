﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GitCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitCommandsTests
{
    [TestClass]
    public class CommitInformationTest
    {
        [TestMethod]
        public void CanCreateCommitInformationFromFormatedData()
        {
            var commitGuid = Guid.NewGuid();
            var treeGuid = Guid.NewGuid();
            var parentGuid1 = Guid.NewGuid();
            var parentGuid2 = Guid.NewGuid();
            var authorTime = DateTime.UtcNow.AddDays(-3);
            var commitTime = DateTime.UtcNow.AddDays(-2);
            var authorUnixTime = (int)(authorTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            var commitUnixTime = (int)(commitTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            var rawData = commitGuid + "\n" +
                          treeGuid + "\n" +
                          parentGuid1 + " " + parentGuid2 + "\n" +
                          "John Doe (Acme Inc) <John.Doe@test.com>\n" + 
                          authorUnixTime + "\n" +
                          "Jane Doe (Acme Inc) <Jane.Doe@test.com>\n" +
                          commitUnixTime + "\n" +
                          "\n" +
                          "\tI made a really neato change.\n\n" +
                          "Notes (p4notes):\n" +
                          "\tP4@547123";

            var expectedHeader = "Author:\t\t<a href='mailto:John.Doe@test.com'>John Doe (Acme Inc) &lt;John.Doe@test.com&gt;</a>" + Environment.NewLine +
                                 "Author date:\t3 days ago (" + authorTime.ToLocalTime().ToString("ddd MMM dd HH':'mm':'ss yyyy") + ")" + Environment.NewLine +
                                 "Committer:\t<a href='mailto:John.Doe@test.com'>Jane Doe (Acme Inc) &lt;Jane.Doe@test.com&gt;</a>" + Environment.NewLine +
                                 "Commit date:\t2 days ago (" + commitTime.ToLocalTime().ToString("ddd MMM dd HH':'mm':'ss yyyy") + ")" + Environment.NewLine +
                                 "Commit hash:\t" + commitGuid;

            var expectedBody = "\n\nI made a really neato change." + Environment.NewLine + Environment.NewLine +
                               "Notes (p4notes):" + Environment.NewLine +
                               "\tP4@547123\n\n";

            var commitData = CommitData.CreateFromFormatedData(rawData);
            var commitInformation = CommitInformation.GetCommitInfo(commitData);
            
            Assert.AreEqual(expectedHeader,commitInformation.Header);
            Assert.AreEqual(expectedBody, commitInformation.Body);
        }
    }
}
