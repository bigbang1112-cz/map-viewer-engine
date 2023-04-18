let scene;
let renderer;
let cam;
let cam_spherical;
let cam_target;
let stats;
let isShiftKeyDown;
let directionalLight;

const solid_instances = [];

function init() {
    
}

function create_3d_renderer() {
    scene = new THREE.Scene();
    
    // Render the scene
    renderer = new THREE.WebGLRenderer();
    renderer.shadowMap.enabled = true;
    renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);

    const canvas = document.querySelector('canvas');
    canvas.parentNode.replaceChild(renderer.domElement, canvas);
}

function track_general_events() {
    // Add event listener to update canvas height when window is resized
    window.addEventListener('resize', function () {
        // Update camera aspect ratio and renderer size
        cam.aspect = window.innerWidth / window.innerHeight;
        cam.updateProjectionMatrix();
        renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);
    }, false);
    
    // Add event listener to handle mouse wheel zoom
    renderer.domElement.addEventListener('wheel', function (event) {
        // Prevent the page from scrolling
        event.preventDefault();

        // Update the radius based on the scroll wheel delta
        cam_spherical.radius += event.deltaY * 0.001 * cam_spherical.radius;

        // Ensure the radius stays within the desired range
        cam_spherical.radius = Math.min(Math.max(cam_spherical.radius, 1), 256);
    });
}

function create_cam(distance) {
    // Create the camera and position it
    cam = new THREE.PerspectiveCamera(90, window.innerWidth / window.innerHeight, 0.1, 1000);
    cam.position.z = -distance;

    cam_target = new THREE.Vector3(0, 0, 0);
    cam_spherical = new THREE.Spherical().setFromVector3(cam.position.clone().sub(cam_target));
    const moveSpeed = 0.002;
    const rotateSpeed = 0.004;

    let isShiftKeyDown = false;
    
    document.addEventListener('keydown', event => {
        if (event.code == "ShiftLeft") {
            isShiftKeyDown = true;
        }
        switch (event.code) {
            case 'KeyA':
                cam_spherical.theta -= rotateSpeed;
                break;
            case 'KeyD':
                cam_spherical.theta += rotateSpeed;
                break;
            case 'KeyS':
                cam_spherical.phi += rotateSpeed;
                break;
            case 'KeyW':
                cam_spherical.phi -= rotateSpeed;
                break;
        }
    });

    document.addEventListener('keyup', event => {
        if (event.code == "ShiftLeft") {
            isShiftKeyDown = false;
        }
    });

    document.addEventListener('mousedown', event => {
        if (event.code == "ShiftLeft" && event.button === 0) {
            isShiftKeyDown = true;
        }
    });

    document.addEventListener('mousemove', event => {
        if (event.buttons === 1) {
            if (isShiftKeyDown) {
                const vector = new THREE.Vector3(-event.movementX * moveSpeed * cam_spherical.radius, event.movementY * moveSpeed * cam_spherical.radius, 0);
                vector.applyQuaternion(cam.quaternion);
                cam_target.add(vector);
            }
            else {
                cam_spherical.theta -= event.movementX * rotateSpeed;
                cam_spherical.phi -= event.movementY * rotateSpeed;
            }
        }
    });
}

function move_cam(x, y, z) {
    cam_target.set(x, y, z);
}

let animationRequestId;

function animate() {
    animationRequestId = requestAnimationFrame(animate);

    // Convert spherical coordinates to cartesian coordinates
    const position = new THREE.Vector3().setFromSpherical(cam_spherical).add(cam_target);

    // Set camera position and look at target
    cam.position.copy(position);
    cam.lookAt(cam_target);

    if (directionalLight != null) {
        directionalLight.position.copy(cam.position)
    }

    stats.begin();
    renderer.render(scene, cam);
    stats.end();
}

function spawn_sample_objects() {
    // Set up the scene and add objects
    var geometry = new THREE.BoxGeometry(1, 1, 1);
    var material = new THREE.MeshStandardMaterial({ color: 0x00ff00 });
    var cube = new THREE.Mesh(geometry, material);
    cube.receiveShadow = true;
    cube.castShadow = true;
    scene.add(cube);

    const planeGeometry = new THREE.PlaneGeometry(10, 10, 10);
    const planeMaterial = new THREE.MeshStandardMaterial({ color: 0xCCCCCC });
    const planeMesh = new THREE.Mesh(planeGeometry, planeMaterial);
    planeMesh.receiveShadow = true;
    planeMesh.castShadow = true;
    planeMesh.position.set(0, -1, 0);
    planeMesh.rotation.x = -Math.PI / 2;

    scene.add(planeMesh);

    // Create an ambient light
    var ambientLight = new THREE.AmbientLight(0x404040);

    // Add the light to the scene
    scene.add(ambientLight);

    // Create a directional light
    directionalLight = new THREE.DirectionalLight(0xffffff, 1);

    // Set the position and direction of the light
    directionalLight.position.set(100, 100, 100);

    // Set up shadow properties for the light
    directionalLight.castShadow = true;
    directionalLight.shadow.mapSize.width = 2048;
    directionalLight.shadow.mapSize.height = 2048;
    directionalLight.shadow.camera.near = 0.1;
    directionalLight.shadow.camera.far = 500;
    directionalLight.shadow.camera.left = -50;
    directionalLight.shadow.camera.right = 50;
    directionalLight.shadow.camera.top = 50;
    directionalLight.shadow.camera.bottom = -50;

    // Add the light to the scene
    scene.add(directionalLight);
}

function create_stats() {
    stats = new Stats();
    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom

    var stats_div = document.getElementById("stats");
    stats_div.parentNode.replaceChild(stats.dom, stats_div);
}

function disposeScene() {
    for (let i = 0; i < solid_instances.length; i++) {
        disposeNode(solid_instances[i], true);
    }
    solid_instances.length = 0;

    while (scene.children.length) {
        scene.remove(scene.children[0]);
    }

    cancelAnimationFrame(animationRequestId);
    renderer.renderLists.dispose();
    renderer.dispose();
    renderer = null;
    scene = null;
}

disposeNode = (node, recursive = false) => {

    if (!node) return;

    if (recursive && node.children)
        for (const child of node.children)
            disposeNode(child, recursive);

    if (node.geometry) {
        node.geometry.dispose();
    }

    if (!node.material) return;

    const materials = node.material.length === undefined ? [node.material] : node.material

    for (const material of materials) {

        for (const key in material) {

            const value = material[key];

            if (value && typeof value === 'object' && 'minFilter' in value)
                value.dispose();

        }

        material && material.dispose();

    }

}