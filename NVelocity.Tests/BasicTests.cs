using NVelocity.App;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace NVelocity.Tests
{
    public class BasicTests
    {
        private string GetSampleData(string name)
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream(name);
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        [Fact]
        public void BasicSubstitution()
        {
            var before = GetSampleData("NVelocity.Tests.TestData.BasicSubstitution.Before.txt");
            var after = GetSampleData("NVelocity.Tests.TestData.BasicSubstitution.After.txt");

            var context = new VelocityContext();

            context.Put("myVariable1", "Bob");
            context.Put("myVariable2", "Kevin");
            context.Put("myVariable3", "Stuart");

            var engine = new VelocityEngine();
            engine.Init();

            var outputWriter = new StringWriter();
            engine.Evaluate(context, outputWriter, GetCurrentMethod(), before);
            var processedText = outputWriter.GetStringBuilder().ToString();

            Assert.Equal(after, processedText);
        }
    }
}
