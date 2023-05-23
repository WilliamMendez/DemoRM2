import MeshFromFunction
import numpy as np
import matplotlib.pyplot as plt

# function = 'x**2 + z**2 - y'
# function = "np.sin(x) + np.cos(y) + np.exp(z)"
function = "x**2 + y**2 + z**2 - 8"

meshgenerator = MeshFromFunction.MarchingCubesMesh(function, -20, 20, 50)

vertices, triangles = meshgenerator.get_mesh()
# print(vertices)

vertices = np.array(vertices)

# plot the vertices
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
ax.scatter(vertices[:, 0], vertices[:, 1], vertices[:, 2], s=1)
plt.show()

# plot the triangles
triangles = np.array(triangles)
print(triangles)
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
ax.plot_trisurf(vertices[:, 0], vertices[:, 1], vertices[:, 2], triangles=triangles)
plt.show()
