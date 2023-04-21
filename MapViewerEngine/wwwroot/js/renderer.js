import * as cam from './cam.js';
import { disposeInstances } from './solid.js';

let renderer;
let animationRequestId;
let scene;

export function create() {
    renderer = new THREE.WebGLRenderer();
    renderer.antialias = true;
    renderer.shadowMap.enabled = true;
    renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);

    const canvas = document.querySelector('canvas');
    canvas.parentNode.replaceChild(renderer.domElement, canvas);

    window.addEventListener('resize', onWindowResize, false);
    renderer.domElement.addEventListener('wheel', onMouseWheel);// Add a click event listener to the renderer element
    renderer.domElement.addEventListener('click', onMeshClick);

    return renderer;
}

async function onMeshClick(event) {
    // Get the mouse coordinates relative to the renderer element
    const mouse = new THREE.Vector2();
    mouse.x = (event.clientX / renderer.domElement.clientWidth) * 2 - 1;
    mouse.y = - ((event.clientY - 80) / renderer.domElement.clientHeight) * 2 + 1;

    // Create a raycaster object
    const raycaster = new THREE.Raycaster();
    raycaster.setFromCamera(mouse, cam.getCam());

    // Find intersected objects
    const intersects = raycaster.intersectObjects(scene.children, true);

    if (intersects.length > 0) {
        // The first intersected object is the closest one
        let clickedObject = intersects[0].object;

        while (clickedObject.parent && clickedObject.parent !== scene) {
            clickedObject = clickedObject.parent;
        }

        console.log(clickedObject.userData);
    }
}

export function createScene() {
    scene = new THREE.Scene();
    return scene;
}

export function addToScene(obj) {
    scene.add(obj);
}

export function animate() {
    animationRequestId = requestAnimationFrame(animate);

    cam.animate();

    //stats.begin();
    renderer.render(scene, cam.getCam());
    //stats.end();
}

function onMouseWheel(event) {
    cam.onMouseWheel(event);
}

function onWindowResize() {
    // Update camera aspect ratio and renderer size
    cam.getCam().aspect = window.innerWidth / window.innerHeight;
    cam.getCam().updateProjectionMatrix();
    renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);
}

export function spawnSampleObjects() {
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
    let directionalLight = new THREE.DirectionalLight(0xffffff, 1);

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

export function dispose() {
    cancelAnimationFrame(animationRequestId);

    window.removeEventListener('resize', onWindowResize);
    renderer.domElement.removeEventListener('wheel', onMouseWheel);
    
    disposeInstances();

    while (scene.children.length) {
        scene.remove(scene.children[0]);
    }
    
    renderer.renderLists.dispose();
    renderer.dispose();
    renderer = null;
    scene = null;
}