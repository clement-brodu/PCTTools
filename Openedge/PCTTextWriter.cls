/*------------------------------------------------------------------------
    File        : PCTTextWriter
    Purpose     : 
    Syntax      : 
    Description : 
    Author(s)   : Clément BRODU
    Created     : 
    Notes       : This file is used to redirect trace from .NET to Openedge
  ----------------------------------------------------------------------*/

BLOCK-LEVEL ON ERROR UNDO, THROW.


CLASS PCTTextWriter 
    INHERITS System.IO.TextWriter: 

    /*------------------------------------------------------------------------------
        Purpose: Disallow instance creation
        Notes:   
    ------------------------------------------------------------------------------*/
    CONSTRUCTOR PUBLIC PCTTextWriter ():
        SUPER ().
        
    END CONSTRUCTOR.

    DEFINE PUBLIC PROPERTY Encoding AS System.Text.Encoding NO-UNDO
    GET:
        RETURN System.Text.Encoding:UTF8.
    END GET.

    METHOD PUBLIC OVERRIDE VOID Write (pcCharacter AS CHARACTER):
        
        MESSAGE pcCharacter. /* will do a WriteLine */

    END METHOD.

    METHOD PUBLIC OVERRIDE VOID WriteLine ():
        
        MESSAGE "".

    END METHOD.
    
    METHOD PUBLIC OVERRIDE VOID WriteLine (pcCharacter AS CHARACTER):
        
        MESSAGE pcCharacter.

    END METHOD.

    METHOD PUBLIC OVERRIDE VOID WriteLine (poObject AS System.Object):
        
        MESSAGE poObject.

    END METHOD.

END CLASS.