Ä+
R/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example/Stacks/CoreStack.cs
	namespace 	
Example
 
. 
Stacks 
; 
internal		 
static			 
class		 
	CoreStack		 
{

 
public 

static 
async 
Task 
< 

Dictionary '
<' (
string( .
,. /
object0 6
?6 7
>7 8
>8 9 
DefineResourcesAsync: N
(N O
stringO U
	stackNameV _
)_ `
{ 
var 
stackConfiguration 
=  
new! $
StackConfiguration% 7
(7 8
)8 9
;9 :
var 
stackReference 
= 
new  
StackReference! /
(/ 0
$"0 2
{2 3
stackConfiguration3 E
.E F!
StackReferenceOrgNameF [
}[ \
$str\ ]
{] ^
stackConfiguration^ p
.p q&
StackReferenceProjectName	q Š
}
Š ‹
$str
‹ Œ
{
Œ 
	stackName
 –
}
– —
"
— ˜
)
˜ ™
;
™ š
object 
? 
stackReferenceValue #
=$ %
await& +
stackReference, :
.: ;
GetValueAsync; H
(H I
$strI q
)q r
;r s
if 

( 
stackReferenceValue 
is  "
not# &
string' -
managedIdentity. =
)= >
{ 	
throw 
new  
InvalidCastException *
(* +
$str+ R
)R S
;S T
} 	
var 
resourceGroup 
= 
new 
ResourceGroup  -
(- .
$str. ?
,? @
newA D
ResourceGroupArgsE V
{ 	
ResourceGroupName 
= 
$str  1
,1 2
Location 
= 
stackConfiguration )
.) *
Location* 2
} 	
)	 

;
 
Vault 
keyVault 
= 
new 0
$KeyVaultWithSecretsComponentResource A
(A B
$strB U
,U V
newW Z5
(KeyVaultWithSecretsComponentResourceArgs	[ ƒ
{ 	
	VaultName 
= 
$" 
$str *
{* +
	stackName+ 4
}4 5
"5 6
,6 7
ResourceGroupName 
= 
resourceGroup  -
.- .
Name. 2
,2 3
TenantId   
=   
stackConfiguration   )
.  ) *
TenantId  * 2
,  2 3
Secrets!! 
=!! 
new!! 
InputMap!! "
<!!" #
string!!# )
>!!) *
{"" 
{## 
$str## -
,##- .
stackConfiguration##/ A
.##A B$
DatabaseConnectionString##B Z
}##Z [
}$$ 
}%% 	
)%%	 

.%%
 
KeyVault%% 
;%% #
GetRoleDefinitionResult'' 
roleDefinition''  .
=''/ 0
await''1 6
GetRoleDefinition''7 H
.''H I
InvokeAsync''I T
(''T U
new''U X!
GetRoleDefinitionArgs''Y n
{(( 	
RoleDefinitionId)) 
=)) 
$str)) E
,))E F
Scope** 
=** 
$"** 
$str** %
{**% &
stackConfiguration**& 8
.**8 9
SubscriptionId**9 G
}**G H
"**H I
}++ 	
)++	 

;++
 
_.. 	
=..
 
await.. 
GetRoleDefinition.. #
...# $
InvokeAsync..$ /
(../ 0
new..0 3!
GetRoleDefinitionArgs..4 I
{// 	
RoleDefinitionId00 
=00 
$str00 E
,00E F
Scope11 
=11 
$"11 
$str11 %
{11% &
stackConfiguration11& 8
.118 9
SubscriptionId119 G
}11G H
"11H I
}22 	
)22	 

;22
 
_44 	
=44
 
new44 
RoleAssignment44 
(44 
$str44 9
,449 :
new44; >
RoleAssignmentArgs44? Q
{55 	
PrincipalId66 
=66 
managedIdentity66 )
,66) *
PrincipalType77 
=77 
PrincipalType77 )
.77) *
ServicePrincipal77* :
,77: ;
RoleDefinitionId88 
=88 
roleDefinition88 -
.88- .
Id88. 0
,880 1
Scope99 
=99 
keyVault99 
.99 
Id99 
}:: 	
)::	 

;::
 
return<< 
new<< 

Dictionary<< 
<<< 
string<< $
,<<$ %
object<<& ,
?<<, -
><<- .
{== 	
{>> 
$str>> 
,>> 
keyVault>> $
.>>$ %

Properties>>% /
.>>/ 0
Apply>>0 5
(>>5 6
x>>6 7
=>>>8 :
x>>; <
.>>< =
VaultUri>>= E
)>>E F
}>>G H
}?? 	
;??	 

}@@ 
}AA °
T/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example/StackConfiguration.cs
	namespace 	
Example
 
; 
internal 
sealed	 
class 
StackConfiguration (
{ 
private 
readonly 
Config 
_defaultConfig *
=+ ,
new- 0
(0 1
)1 2
;2 3
private 
readonly 
Config 
_azureNativeConfig .
=/ 0
new1 4
(4 5
$str5 C
)C D
;D E
public

 

string

 
TenantId

 
=>

 
_azureNativeConfig

 0
.

0 1
Require

1 8
(

8 9
$str

9 C
)

C D
;

D E
public 

string 
SubscriptionId  
=>! #
_azureNativeConfig$ 6
.6 7
Require7 >
(> ?
$str? O
)O P
;P Q
public 

string 
Location 
=> 
_azureNativeConfig 0
.0 1
Require1 8
(8 9
$str9 C
)C D
;D E
public 

string !
StackReferenceOrgName '
=>( *
_defaultConfig+ 9
.9 :
Require: A
(A B
$strB Y
)Y Z
;Z [
public 

string %
StackReferenceProjectName +
=>, .
_defaultConfig/ =
.= >
Require> E
(E F
$strF a
)a b
;b c
public 

Output 
< 
string 
> $
DatabaseConnectionString 2
=>3 5
_defaultConfig6 D
.D E
RequireSecretE R
(R S
$strS m
)m n
;n o
} ÷
I/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example/Program.cs
return 
await 

Deployment 
. 
RunAsync  
(  !
async! &
(' (
)( )
=>* ,
await- 2
	CoreStack3 <
.< = 
DefineResourcesAsync= Q
(Q R

DeploymentR \
.\ ]
Instance] e
.e f
	StackNamef o
)o p
)p q
;q rÌ0
y/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example/ComponentResources/KeyVaultWithSecretsComponentResource.cs
	namespace 	
Example
 
. 
ComponentResources $
;$ %
internal 
sealed	 
class 0
$KeyVaultWithSecretsComponentResource :
:; <
ComponentResource= N
{ 
public		 
0
$KeyVaultWithSecretsComponentResource		 /
(		/ 0
string		0 6
name		7 ;
,		; <4
(KeyVaultWithSecretsComponentResourceArgs		= e
args		f j
,		j k%
ComponentResourceOptions			l „
?
		„ …
options
		† 
=
		Ž 
null
		 ”
)
		” •
:

 	
base


 
(

 
$str

 D
,

D E
name

F J
,

J K
options

L S
)

S T
{ 
var 
keyVault 
= 
new 
Vault  
(  !
$"! #
{# $
name$ (
}( )
$str) ,
", -
,- .
new/ 2
(2 3
)3 4
{ 	
	VaultName 
= 
args 
. 
	VaultName &
,& '
ResourceGroupName 
= 
args  $
.$ %
ResourceGroupName% 6
,6 7

Properties 
= 
new 
VaultPropertiesArgs 0
{ #
EnableRbacAuthorization '
=( )
true* .
,. /
Sku 
= 
new 
SkuArgs !
{ 
Family 
= 
	SkuFamily &
.& '
A' (
,( )
Name 
= 
SkuName "
." #
Standard# +
} 
, 
TenantId 
= 
args 
.  
TenantId  (
} 
} 	
,	 

new 
( 
) 
{ 
Parent 
= 
this  
}! "
)" #
;# $
if 

( 
args 
. 
Secrets 
is 
not 
null  $
)$ %
{ 	
args 
. 
Secrets 
. 
Apply 
( 
secrets &
=>' )
{ 
foreach   
(   
KeyValuePair   %
<  % &
string  & ,
,  , -
string  . 4
>  4 5
kv  6 8
in  9 ;
secrets  < C
)  C D
{!! 
_"" 
="" 
new"" 
Secret"" "
(""" #
$"""# %
{""% &
name""& *
}""* +
$str""+ 3
{""3 4
kv""4 6
.""6 7
Key""7 :
}"": ;
"""; <
,""< =
new""> A

SecretArgs""B L
{## 

SecretName$$ "
=$$# $
kv$$% '
.$$' (
Key$$( +
,$$+ ,

Properties%% "
=%%# $
new%%% ( 
SecretPropertiesArgs%%) =
{&& 
Value'' !
=''" #
kv''$ &
.''& '
Value''' ,
}(( 
,(( 
ResourceGroupName)) )
=))* +
args)), 0
.))0 1
ResourceGroupName))1 B
,))B C
	VaultName** !
=**" #
keyVault**$ ,
.**, -
Name**- 1
}++ 
,++ 
new++ !
CustomResourceOptions++ 0
{++1 2
Parent++3 9
=++: ;
this++< @
}++A B
)++B C
;++C D
},, 
return.. 
secrets.. 
;.. 
}// 
)// 
;// 
}00 	
KeyVault22 
=22 
keyVault22 
;22 
RegisterOutputs33 
(33 
new33 

Dictionary33 &
<33& '
string33' -
,33- .
object33/ 5
?335 6
>336 7
{44 	
{55 
$str55 
,55 
keyVault55 "
}55# $
}66 	
)66	 

;66
 
}77 
public99 

Vault99 
KeyVault99 
{99 
get99 
;99  
}99! "
}:: 
internal<< 
sealed<<	 
class<< 4
(KeyVaultWithSecretsComponentResourceArgs<< >
:<<? @
ResourceArgs<<A M
{== 
[>> 
Input>> 

(>>
 
$str>> 
,>> 
required>>  
:>>  !
true>>" &
)>>& '
]>>' (
public?? 

required?? 
Input?? 
<?? 
string??  
>??  !
	VaultName??" +
{??, -
get??. 1
;??1 2
init??3 7
;??7 8
}??9 :
[AA 
InputAA 

(AA
 
$strAA 
,AA 
requiredAA  (
:AA( )
trueAA* .
)AA. /
]AA/ 0
publicBB 

requiredBB 
InputBB 
<BB 
stringBB  
>BB  !
ResourceGroupNameBB" 3
{BB4 5
getBB6 9
;BB9 :
initBB; ?
;BB? @
}BBA B
[DD 
InputDD 

(DD
 
$strDD 
,DD 
requiredDD 
:DD  
trueDD! %
)DD% &
]DD& '
publicEE 

requiredEE 
InputEE 
<EE 
stringEE  
>EE  !
TenantIdEE" *
{EE+ ,
getEE- 0
;EE0 1
initEE2 6
;EE6 7
}EE8 9
[GG 
InputGG 

(GG
 
$strGG 
)GG 
]GG 
publicHH 

InputMapHH 
<HH 
stringHH 
>HH 
?HH 
SecretsHH $
{HH% &
getHH' *
;HH* +
initHH, 0
;HH0 1
}HH2 3
}II 