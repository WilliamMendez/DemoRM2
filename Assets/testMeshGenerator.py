import MarchingCubesMesh as MeshFromFunction
import numpy as np
from mpl_toolkits.mplot3d.art3d import Poly3DCollection
import matplotlib.pyplot as plt

function = 'x**2 + z**2 - y'
# function = "np.sin(x) + np.cos(y) + np.exp(z)"
# function = "x**2 + y**2 + z**2"

meshgenerator = MeshFromFunction.MarchingCubesMesh()


l = -20
u = 20
resolution = 10
verts, triangles = meshgenerator.getFromFunction(function, l, u, resolution)
# print(vertices)

# # los vertices están de la forma: [{"x": 0, "y": 0, "z": 0}, {"x": 0, "y": 0, "z": 0}, ...  }]
# vertices = np.array([[v["x"], v["y"], v["z"]] for v in verts])

# # plot the vertices
# fig = plt.figure()
# ax = fig.add_subplot(111, projection='3d')
# ax.scatter(vertices[:, 0], vertices[:, 1], vertices[:, 2], s=1)
# plt.show()

# plot the triangles (faces) of the mesh adjusted to the range
plt.figure()
ax = plt.axes(projection='3d')
ax.set_xlim(l, u)
ax.set_ylim(l, u)
ax.set_zlim(l, u)
ax.set_xlabel('x')
ax.set_ylabel('y')
ax.set_zlabel('z')
ax.plot_trisurf(verts[:, 0], verts[:, 1], verts[:, 2], triangles=triangles, cmap='viridis', edgecolor='none')
plt.show()


# triangles = np.array(triangles)
# # print(triangles)
# fig = plt.figure()
# ax = fig.add_subplot(111, projection='3d')
# ax.plot_trisurf(vertices[:, 0], vertices[:, 1], vertices[:, 2], triangles=triangles)
# plt.show()
