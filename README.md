## Solution moving parts

### Fody Project

#### Assembly Name

The project that does the weaving. It outputs a file named SampleFodyAddin.Fody.

#### ModuleWeaver

ModuleWeaver.cs is where the target assembly is modified. Fody will pick up this type during a its processing.

In this case a new type is being injected into the target assemlby that looks like this.

	public class Hello
	{
	    public string World()
	    {
	        return "Hello World";
	    }
	}

See [ModuleWeaver](https://github.com/SimonCropp/Fody/wiki/ModuleWeaver)
 for more details.

### Nuget Project

Fody addins are deployed as [nuget](http://nuget.org/) packages. NugetProject builds the package for SampleFodyAddin as part of a build. The output of this project is placed in *SolutionDir*/NuGetBuild. 

This project uses  [pepita](https://code.google.com/p/pepita/) to construct the package but you could also use nuget.exe.

For more information on the nuget structure of Fody addins see [DeployingAddinsAsNugets](https://github.com/SimonCropp/Fody/wiki/DeployingAddinsAsNugets)


### AssemblyToProcess Project

A target assembly to process and then validate with unit tests.

### Tests  Project

The test assembly contains three parts

#### 1. WeaverHelper

A helper class that takes the output of  AssemblyToProcess and uses ModuleWeaver to process it. It also create a copy of the target assembly suffixed with '2' so a side-by-side comparison of the before and after IL can be done using a decompiler.

#### 2. Verifier

A helper class that runs [peverfiy](http://msdn.microsoft.com/en-us/library/62bwd2yd(v=vs.110).aspx) to validate the resultant assembly.

#### 3. Tests

The actual unit tests that use WeaverHelper and Verifier. It has one test to construct and execute the injected class.

