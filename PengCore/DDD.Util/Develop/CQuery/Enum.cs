﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Util.CQuery
{
    /// <summary>
    /// Condition Operator
    /// </summary>
    public enum CriteriaOperator
    {
        Equal,              //=  
        NotEqual,      //<>  
        LessThanOrEqual,    //<=  
        LessThan,           //<  
        GreaterThan,        //>  
        GreaterThanOrEqual, //>=  
        In,                 //IN()  
        NotIn,              //NOT IN ()  
        Like,
        BeginLike,
        EndLike
    }

    /// <summary>
    /// Connect Operator
    /// </summary>
    public enum QueryOperator
    {
        AND,
        OR
    }

    /// <summary>
    /// Query Command Type
    /// </summary>
    public enum QueryCommandType
    {
        QueryObject,
        Text
    }


    /// <summary>
    /// Calculate Operator
    /// </summary>
    public enum CalculateOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
