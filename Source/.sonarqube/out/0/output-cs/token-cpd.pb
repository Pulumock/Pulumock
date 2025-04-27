Ý

P/Users/robertopiranamedi/RiderProjects/Pulumock/Source/StackReference/Program.cs
return 
await 
Pulumi 
. 

Deployment 
. 
RunAsync '
(' (
(( )
)) *
=>+ -
{ 
var 
resourceGroup 
= 
new 
ResourceGroup )
() *
$str* 7
,7 8
new9 <
(< =
)= >
{ 
ResourceGroupName 
= 
$str )
}		 
)		 
;		 
var 
managedIdentity 
= 
new  
UserAssignedIdentity 2
(2 3
$str3 D
,D E
newF I$
UserAssignedIdentityArgsJ b
{ 
ResourceGroupName 
= 
resourceGroup )
.) *
Name* .
} 
) 
; 
return 

new 

Dictionary 
< 
string  
,  !
object" (
?( )
>) *
{ 
{ 	
$str	 1
,1 2
managedIdentity3 B
.B C
PrincipalIdC N
}O P
} 
; 
} 
) 
; 