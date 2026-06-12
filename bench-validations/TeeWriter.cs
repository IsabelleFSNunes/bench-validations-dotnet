using System;
using System.Collections.Generic;
using System.Text;

namespace bench_validations
{
    // =================================================================
    // TEE WRITER —  To write on console and .out file simultaneously
    // =================================================================
    public class TeeWriter(TextWriter primary, TextWriter secondary) : TextWriter
    {
        public override Encoding Encoding => primary.Encoding;
        public override void Write(char value) { primary.Write(value); secondary.Write(value); }
        public override void WriteLine(string? value) { primary.WriteLine(value); secondary.WriteLine(value); }
        public override void Flush() { primary.Flush(); secondary.Flush(); }
    }

}
