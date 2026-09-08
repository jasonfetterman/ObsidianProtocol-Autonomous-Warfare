using System.IO;
using System.Text;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace MiniRTS.EditorTools
{
    /// Runs EditMode tests inside the live editor and writes a plain-text
    /// report, so the CLI pipeline can trigger tests without a second editor
    /// instance fighting over the project lock.
    public static class HeadlessTestRunner
    {
        public const string OutPath = "/tmp/minirts_test_results.txt";

        class Report : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun) { }

            public void RunFinished(ITestResultAdaptor result)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"RUN_FINISHED status={result.TestStatus} passed={result.PassCount} failed={result.FailCount} skipped={result.SkipCount}");
                Collect(result, sb);
                File.WriteAllText(OutPath, sb.ToString());
            }

            static void Collect(ITestResultAdaptor r, StringBuilder sb)
            {
                if (!r.HasChildren)
                {
                    sb.AppendLine($"{r.TestStatus}: {r.FullName}");
                    if (r.TestStatus == TestStatus.Failed)
                        sb.AppendLine($"  MSG: {r.Message}\n  {r.StackTrace}");
                    return;
                }
                foreach (var c in r.Children) Collect(c, sb);
            }

            public void TestStarted(ITestAdaptor test) { }
            public void TestFinished(ITestResultAdaptor result) { }
        }

        public static string RunEditMode()
        {
            File.Delete(OutPath);
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(new Report());
            api.Execute(new ExecutionSettings(new Filter { testMode = TestMode.EditMode }));
            return "EditMode test run started; results at " + OutPath;
        }
    }
}
