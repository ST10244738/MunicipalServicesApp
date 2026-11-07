Municipal Services Application
1. Overview
A C# Windows Forms desktop prototype for reporting municipal issues and tracking service requests. It also demonstrates trees, a max heap and graph algorithms.
2. Technologies
- C#
- Windows Forms
- .NET target framework: check the project properties (the project has been developed as a .NET Windows Forms application).
- Visual Studio 2022
- In-memory repository
3. Main project files
- MainForm.cs – main menu and navigation.
- ReportIssueForm.cs – report an issue, select a category, enter details and optionally choose an attachment.
- EventsForm.cs – local events, search, filters and recommendations.
- ServiceRequestStatusForm.cs – request table, search, summary cards, details and status history.
- Models/ – issue and service-request models, enums and repository.
- DataStructures/ – BasicTree, BinaryTree, BinarySearchTree, AVLTree, RedBlackTree, MaxHeap, Graph and ServiceRequestDataManager.
- ServiceRequestDataManagerTest.cs – integration test method for the data structures.
Use Solution Explorer to confirm the exact file names and folders in your copy of the project.
4. Requirements
- Windows computer.
- Visual Studio 2022 with the .NET desktop development workload.
- .NET SDK/runtime compatible with the target framework shown in the project properties.
- The solution (.sln) and all project source files.
5. Compile/build instructions
1. Open Visual Studio 2022.
2. Select File → Open → Project/Solution.
3. Open the project's .sln file.
4. If prompted, restore NuGet packages.
5. Select Build → Build Solution or press Ctrl+Shift+B.
6. Review the Error List and resolve any build errors.
7. Confirm that the build completes successfully.
6. Run instructions
1. In Solution Explorer, right-click the MunicipalServicesApp project.
2. Select Set as Startup Project.
3. Press F5 or select Debug → Start Without Debugging (Ctrl+F5).
4. The main menu should open.
5. Use the menu buttons to open the reporting, events and service-request status features.
7. How to use the application
7.1 Report an issue
1. Open Report Issue.
2. Enter the issue location.
3. Select a category: Roads, Water Supply, Electricity, Sanitation, Waste Management, Street Lighting, Public Safety or Other.
4. Enter a description.
5. Optionally select Attach File and choose an image or document.
6. Select Submit Issue.
7. Record the generated reference, for example SR-2026-0006.
8. The request is assigned to a department and created with Submitted status and Normal priority.
Department mapping:
- Roads → Roads
- Water Supply / Sanitation → WaterAndSanitation
- Electricity / Street Lighting → Electricity
- Waste Management → WasteManagement
- Public Safety → PublicSafety
- Other → GeneralServices
7.2 Track a request
1. Open Service Request Status.
2. Find the request in the table or enter its reference number in the search box.
3. Select Search to filter by reference.
4. Select Show All to restore the full list.
5. Select a row to view its details and status history.
6. Review the summary cards and most urgent request indicator.
7.3 Events and announcements
1. Open the events feature from the main menu.
2. Search with a keyword.
3. Apply the available category and date filters.
4. Review the recommendations.
5. Recommendation scoring combines search preferences and event priority.
8. Data structures
- Basic tree – department hierarchy and DFS/BFS traversal.
- Binary tree – binary-tree storage and traversal.
- Binary search tree – reference-based searching and sorted retrieval.
- AVL tree – self-balancing search tree.
- Red-Black tree – balanced search tree.
- Max heap – priority-based request selection.
- Graph – department vertices, BFS, DFS and Prim's minimum spanning tree.
The integration test adds sample weighted graph edges before running traversal and MST tests. These are algorithm demonstration data, not live municipal route data.
9. Integration tests
The project includes ServiceRequestDataManagerTest.cs with a RunTests() method. If it is not connected to a test runner, call it temporarily from a suitable test entry point, run the project and inspect Visual Studio's Output window. Remove any temporary test invocation before normal use. The recorded integration run checked repository loading, tree searches/traversals, heap priority, graph BFS/DFS and Prim's MST.
10. Limitations
- Requests are stored in a static in-memory repository. They are lost when the application closes.
- The attachment path is currently stored in the original Issue record; it is not displayed or persisted by the ServiceRequest model.
- New submissions use the default resident label Resident, Submitted status and Normal priority.
- The prototype does not currently include persistent database storage or resident authentication.
11. Troubleshooting
- Build errors: Check the project's target framework and installed .NET SDK, restore packages and rebuild.
- A form does not open: Confirm it is included in the project and the navigation button opens the correct form.
- A new request is not visible: Confirm ServiceRequestRepository.Add() is called during submission and the status screen reloads repository records.
- Requests disappear after closing: This is expected with the current in-memory repository.
