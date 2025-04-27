æ*
d/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithPulumock/Shared/TestBase.cs
	namespace

 	
Example


 
.

 
Tests

 
.

 
WithPulumock

 $
.

$ %
Shared

% +
;

+ ,
public 
class 
TestBase 
{ 
	protected 
TestBase 
( 
) 
=> 
FixtureBuilder 
= 
new 
FixtureBuilder +
(+ ,
), -
. !
WithMockConfiguration "
(" #
new# &$
MockConfigurationBuilder' ?
(? @
)@ A
. 
WithConfiguration "
(" #(
PulumiConfigurationNamespace# ?
.? @
AzureNative@ K
,K L
$strM W
,W X
$strY 
)	 €
. 
WithConfiguration "
(" #(
PulumiConfigurationNamespace# ?
.? @
AzureNative@ K
,K L
$strM ]
,] ^
$str	_ …
)
… †
. 
WithConfiguration "
(" #(
PulumiConfigurationNamespace# ?
.? @
AzureNative@ K
,K L
$strM W
,W X
$strY h
)h i
. 
WithConfiguration "
(" #(
PulumiConfigurationNamespace# ?
.? @
Default@ G
,G H
$strI `
,` a
$strb j
)j k
. 
WithConfiguration "
(" #(
PulumiConfigurationNamespace# ?
.? @
Default@ G
,G H
$strI d
,d e
$strf v
)v w
. #
WithSecretConfiguration (
(( )(
PulumiConfigurationNamespace) E
.E F
DefaultF M
,M N
$strO i
,i j
$strk ~
)~ 
. 
Build 
( 
) 
) 
. "
WithMockStackReference #
(# $
new$ '%
MockStackReferenceBuilder( A
(A B
$"B D
$strD Z
{Z [
	StackName[ d
}d e
"e f
)f g
. 

WithOutput 
( 
$str D
,D E
$strF l
)l m
. 
Build 
( 
) 
) 
. 
WithMockResource 
( 
new !
MockResourceBuilder" 5
(5 6
)6 7
. 

WithOutput 
< 
ResourceGroup )
>) *
(* +
x+ ,
=>- /
x0 1
.1 2
AzureApiVersion2 A
,A B
$strC O
)O P
. 

WithOutput 
< 
ResourceGroup )
>) *
(* +
x+ ,
=>- /
x0 1
.1 2
Location2 :
,: ;
$str< D
)D E
.   
Build   
<   
ResourceGroup   $
>  $ %
(  % &
)  & '
)  ' (
.!! 
WithMockResource!! 
(!! 
new!! !
MockResourceBuilder!!" 5
(!!5 6
)!!6 7
."" 

WithOutput"" 
<"" 
Vault"" !
,""! "#
VaultPropertiesResponse""# :
>"": ;
(""; <
x""< =
=>""> @
x""A B
.""B C

Properties""C M
,""M N
p""O P
=>""Q S
p""T U
.## 
WithNestedOutput## %
(##% &
x##& '
=>##( *
x##+ ,
.##, -
VaultUri##- 5
,##5 6
$str##7 X
)##X Y
)##Y Z
.$$ 
Build$$ 
<$$ 
Vault$$ 
>$$ 
($$ 
)$$ 
)$$  
.%% 
WithMockCall%% 
(%% 
new%% 
MockCallBuilder%% -
(%%- .
)%%. /
.&& 

WithOutput&& 
<&& #
GetRoleDefinitionResult&& 3
>&&3 4
(&&4 5
x&&5 6
=>&&7 9
x&&: ;
.&&; <
Id&&< >
,&&> ?
$str&&@ f
)&&f g
.'' 

WithOutput'' 
<'' #
GetRoleDefinitionResult'' 3
,''3 4
PermissionResponse''5 G
>''G H
(''H I
x''I J
=>''K M
x''N O
.''O P
Permissions''P [
,''[ \
p''] ^
=>''_ a
p(( 
.(( 
WithNestedOutput(( &
(((& '
x((' (
=>(() +
x((, -
.((- .
	Condition((. 7
,((7 8
$str((9 D
)((D E
)((E F
.)) 
Build)) 
()) 
typeof)) 
()) 
GetRoleDefinition)) /
)))/ 0
)))0 1
)))1 2
;))2 3
	protected** 
FixtureBuilder** 
FixtureBuilder** +
{**, -
get**. 1
;**1 2
}**3 4
	protected,, 
const,, 
string,, 
	StackName,, $
=,,% &
$str,,' ,
;,,, -
}-- c
a/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithPulumock/GlobalUsings.csª

g/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithPulumock/ConfigurationTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithPulumock $
;$ %
public 
class 
ConfigurationTests 
:  !
TestBase" *
,* +
IConfigurationTests, ?
{		 
[

 
Fact

 	
]

	 

public 

async 
Task 0
$Config_MockedConfigurationInResource :
(: ;
); <
{ 
Fixture 
fixture 
= 
await 
FixtureBuilder  .
. 

BuildAsync 
( 
async 
( 
)  
=>! #
await$ )
	CoreStack* 3
.3 4 
DefineResourcesAsync4 H
(H I
	StackNameI R
)R S
)S T
;T U
}   
["" 
Fact"" 	
]""	 

public## 

Task## )
Config_MockedSecretInResource## -
(##- .
)##. /
=>##0 2
throw##3 8
new##9 <#
NotImplementedException##= T
(##T U
)##U V
;##V W
}$$ 