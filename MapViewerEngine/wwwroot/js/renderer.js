import * as cam from './cam.js';
import { disposeInstances } from './solid.js';
import * as shader from './shader.js';

let renderer;
let animationRequestId;
let scene;

export function create() {
    renderer = new THREE.WebGLRenderer();
    renderer.antialias = true;
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);

    const canvas = document.querySelector('canvas');
    canvas.parentNode.replaceChild(renderer.domElement, canvas);

    window.addEventListener('resize', onWindowResize, false);
    renderer.domElement.addEventListener('wheel', onMouseWheel);
    renderer.domElement.addEventListener('click', onMeshClick);
    renderer.domElement.addEventListener('contextmenu', onContextMenu);
    renderer.domElement.addEventListener('mousedown', onMouseDown);
    renderer.domElement.addEventListener('mouseup', onMouseUp);
    renderer.domElement.addEventListener('mousemove', onMouseMove);
    renderer.domElement.addEventListener("selectstart", function (event) {
        event.preventDefault();
    });

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
    scene.background = new THREE.CubeTextureLoader()
        .setPath('_content/MapViewerEngine/textures/skybox/')
        .load(['right.webp', 'left.webp', 'top.webp', 'bottom.webp', 'front.webp', 'back.webp']);
    return scene;
}

export function addToScene(obj) {
    scene.add(obj);
}

export function animate(t) {
    animationRequestId = requestAnimationFrame(animate);
    
    cam.animate();

    //stats.begin();
    shader.animate();
    renderer.render(scene, cam.getCam());
    //stats.end();
}

function onMouseWheel(event) {
    cam.onMouseWheel(event);
}

function onMouseDown(event) {
    //renderer.domElement.requestPointerLock();
    cam.onMouseDown(event);
}

function onMouseUp(event) {
    //document.exitPointerLock();
    cam.onMouseUp(event);
}

function onMouseMove(event) {
    cam.onMouseMove(event);
}

function onWindowResize() {
    // Update camera aspect ratio and renderer size
    cam.getCam().aspect = window.innerWidth / window.innerHeight;
    cam.getCam().updateProjectionMatrix();
    renderer.setSize(window.innerWidth - 10, window.innerHeight - 80);
}

function onContextMenu(event) {
    event.preventDefault();
}

export function spawnSampleObjects() {
    // Create an ambient light
    var ambientLight = new THREE.AmbientLight(0x7F7F7F);

    // Add the light to the scene
    scene.add(ambientLight);

    // Create a directional light
    let directionalLight = new THREE.DirectionalLight(0xffffff, 1);

    // Set the position and direction of the light
    directionalLight.position.set(0, 256, 0);
    directionalLight.target.position.set(-2048, 0, -2048);

    // Set up shadow properties for the light
    directionalLight.castShadow = true;
    directionalLight.shadow.mapSize.width = 4096;
    directionalLight.shadow.mapSize.height = 4096;
    directionalLight.shadow.camera.left = -5000;
    directionalLight.shadow.camera.right = 5000;
    directionalLight.shadow.camera.top = 5000;
    directionalLight.shadow.camera.bottom = -5000;
    directionalLight.shadow.camera.near = 0.1;
    directionalLight.shadow.camera.far = 1000;

    directionalLight.shadow.camera.up.set(0, 0, 1);
    directionalLight.shadow.camera.lookAt(0, 0, 0);

    // Add the light to the scene
    scene.add(directionalLight);
}

export function dispose() {
    cancelAnimationFrame(animationRequestId);

    window.removeEventListener('resize', onWindowResize);
    renderer.domElement.removeEventListener('wheel', onMouseWheel);
    renderer.domElement.removeEventListener('click', onMeshClick);
    renderer.domElement.removeEventListener('contextmenu', onContextMenu);
    renderer.domElement.removeEventListener('mousedown', onMouseDown);
    renderer.domElement.removeEventListener('mouseup', onMouseUp);

    disposeInstances();

    while (scene.children.length) {
        scene.remove(scene.children[0]);
    }
    
    cam.dispose();
    renderer.renderLists.dispose();
    renderer.dispose();
    renderer = null;
    scene = null;
}