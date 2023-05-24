from fastapi import FastAPI
import uvicorn
from MarchingCubesMesh import MarchingCubesMesh
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

origins = ["*"]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"]
)

@app.on_event("startup")
async def startup_event():
    global mc_mesh
    mc_mesh = MarchingCubesMesh()

@app.get("/")
def read_root():
    return {"Hello": "World ඞ"}


@app.get("/generate_mesh")
async def generate_mesh_route(function: str, l: float, u: float, resolution: int):
    mc_mesh.getFromFunction(function, l, u, resolution)
    vertices, triangles = mc_mesh.vertices, mc_mesh.triangles
    return {"vertices": vertices, "triangles": triangles}

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
