using System;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public interface IInitialize
    {
    }

    public interface IInitialize<in TInit> : IInitialize
    {
        void Initialize(TInit init);
    }

    public interface IInitialize<in TInit1, in TInit2> : IInitialize
    {
        void Initialize(TInit1 init1, TInit2 init2);
    }

    public interface IInitialize<in TInit1, in TInit2, in TInit3> : IInitialize
    {
        void Initialize(TInit1 init1, TInit2 init2, TInit3 init3);
    }

    public interface IInitialize<in TInit1, in TInit2, in TInit3, in TInit4> : IInitialize
    {
        void Initialize(TInit1 init1, TInit2 init2, TInit3 init3, TInit4 init4);
    }
}