using System.Collections;
using System.Collections.Generic;
using System;

public class NotFoundException : Exception {
	public NotFoundException(string message) : base (message) { } // : base (message) calls the super constructor
}
