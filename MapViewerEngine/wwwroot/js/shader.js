const textureLoader = new THREE.TextureLoader();

function createBlendShader(texture1, texture2, blendMap) {
    return new THREE.ShaderMaterial({
        uniforms: {
            texture1: { value: texture1 },
            texture2: { value: texture2 },
            blendMap: { value: blendMap },
            blendAmount: { value: 1 }
        },
        vertexShader: `
    varying vec2 vUv;

    void main() {
      vUv = uv;
      gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }
  `,
        fragmentShader: `
    uniform sampler2D texture1;
    uniform sampler2D texture2;
    uniform sampler2D blendMap;
    uniform float blendAmount;

    varying vec2 vUv;

    void main() {
      vec4 texel1 = texture2D(texture1, vUv);
      vec4 texel2 = texture2D(texture2, vUv);
      vec4 blendTexel = texture2D(blendMap, vUv);

      // Mix the two textures using the blend map
      gl_FragColor = mix(texel1, texel2, blendTexel.r * blendAmount);
    }
  `
    });
}

var waterMaterial = new THREE.MeshStandardMaterial({
    color: 0x4D8ED0,
    transparent: true,
    opacity: 0.5
});

export function create(name) {

    if (name == "SpeedWater"
        || name == "BaySea"
        || name == "BayWarpSea"
        || name == "CoastSea"
        || name == "CoastWarpSea"
        || name == "CoastFoam"
        || name == "RallyWater"
        || name == "RallyWarpLake"
        || name == "StadiumWater"
        || name == "IslandSea"
        || name == "IslandWarpSea") {
        return waterMaterial;
    }

    let color = 0xBBBBBB;
    let opacity = 1;
    let transparent = false;

    if (name == "StadiumGrassFence" || name.includes("Collision")) {
        opacity = 0;
        transparent = true;
    }

    var material = new THREE.MeshStandardMaterial({
        color: color,
        opacity: opacity,
        transparent: transparent
    });
    material.name = name;
    return material;
}

export function set(mesh, shader) {
    mesh.material = shader;
}

export function loadTexture(path) {
    const texture = textureLoader.load(path);
    texture.wrapS = THREE.RepeatWrapping;
    texture.wrapT = THREE.RepeatWrapping;
    return texture;
}

export function setTexture(shader, texture, name) {
    if (name == "Diffuse" || name == "Soil" || name == "Advert" || name == "Grass" || name == "Blend1") {
        if (shader.map == null) {
            shader.map = texture;
            shader.needsUpdate = true;
            return true;
        }
    }
    else if (name == "Normal") {
        //shader.normalMap = texture;
    }
    else {
        console.log(name);
    }

    return false;
}

export function animate() {
    
}