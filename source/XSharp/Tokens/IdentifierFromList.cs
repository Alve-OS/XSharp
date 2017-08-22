﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public abstract class IdentifierFromList : Identifier {
    protected string[] mList;

    protected IdentifierFromList(bool aUpper = true) : base(null, aUpper) {
      mList = GetList();
    }

    protected abstract string[] GetList();

    protected override object IsMatch(object aValue) {
      if (aValue is string && mList.Contains((string)aValue)) {
        return aValue;
      }
      return null;
    }
  }
}
