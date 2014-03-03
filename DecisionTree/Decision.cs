using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionTree
{
    public abstract class Decision<TInput, TResult> 
    {
        public abstract TResult Evaluate(TInput evaluatedObject);
    }

    public abstract class ResultDecision<TInput, TResult> : Decision<TInput, TResult> 
    {
        // immutable for functional implementation
        readonly bool _result;

        public ResultDecision(bool result)
        {
            _result = result;
        }

        public bool Result { get { return _result; } }
    }

    public class BranchDecision<TInput, TResult> : Decision<TInput, TResult> 
    {
        public BranchDecision(string title, Func<TInput, bool> validation, Decision<TInput, TResult> positive, Decision<TInput, TResult> negative)
        {
            _title = title;
            _positive = positive;
            _negative = negative;
            _validation = validation;
        }

        // immutable for functional implementation
        readonly string _title;
        readonly Decision<TInput, TResult> _positive;
        readonly Decision<TInput, TResult> _negative;
        readonly Func<TInput, bool> _validation;

        public string Title { get { return _title; } }
        public Decision<TInput, TResult> Positive { get { return _positive; } }
        public Decision<TInput, TResult> Negative { get { return _negative; } }
        public Func<TInput, bool> Validation { get { return _validation; } }

        public override TResult Evaluate(TInput evaluatedObject) 
        {
            bool res = Validation(evaluatedObject);

            Decision<TInput, TResult> next = res ? Positive : Negative;
            return next.Evaluate(evaluatedObject);
        }

    }
}
