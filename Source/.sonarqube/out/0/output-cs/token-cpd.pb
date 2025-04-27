ý
n/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/IStackReferenceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface  
IStackReferenceTests %
{ 
Task B
6StackReference_ShouldUseMockedStackReferenceInResource	 ?
(? @
)@ A
;A B
} å
k/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/IStackOutputTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface 
IStackOutputTests "
{ 
Task 0
$StackOutputs_ShouldOutputMockedValue	 -
(- .
). /
;/ 0
} Ì
h/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/IResourceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface 
IResourceTests 
{		 
Task 
Resource_InputOnly	 
( 
) 
; 
Task 
Resource_OutputOnly	 
( 
) 
; 
Task%%  
Resource_InputOutput%%	 
(%% 
)%% 
;%%  
Task++ 
Resource_Dependency++	 
(++ 
)++ 
;++ 
Task00 
Resource_Multiple00	 
(00 
)00 
;00 
}11 Ó
m/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/IConfigurationTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface 
IConfigurationTests $
{ 
Task 0
$Config_MockedConfigurationInResource	 -
(- .
). /
;/ 0
Task )
Config_MockedSecretInResource	 &
(& '
)' (
;( )
} ¥
q/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/IComponentResourceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface #
IComponentResourceTests (
{ 
Task 
ComponentResource	 
( 
) 
; 
Task ;
/ComponentResource_MissingNonRequiredResourceArg	 8
(8 9
)9 :
;: ;
Task $
ComponentResource_Parent	 !
(! "
)" #
;# $
Task %
ComponentResource_Outputs	 "
(" #
)# $
;$ %
}		 ù
d/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.Shared/Interfaces/ICallTests.cs
	namespace 	
Example
 
. 
Tests 
. 
Shared 
. 

Interfaces )
;) *
public 
	interface 

ICallTests 
{ 
Task 

Call_Input	 
( 
) 
; 
Task 
Call_Output	 
( 
) 
; 
Task #
Call_ResourceDependency	  
(  !
)! "
;" #
} 