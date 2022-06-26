# Lessons #

1. Started with every model class as an interface.
Realized that, in many cases, this will lead to excessive boilerplate.
    
	**Action:**
	
	Only algorithms/services that might become dependencies, or have behavior that will need mocking need extra abstraction.

