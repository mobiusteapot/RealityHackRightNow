https://devpost.com/software/root-xr?_gl=1*1mytrqz*_gcl_au*NDc5Mjk5MDQ3LjE3NDQ0ODU0MTA.*_ga*MjA2NzM1NzExMS4xNzQ0NDg1NDEx*_ga_0YHJK3Y10M*MTc0NTc3MDQ5My4yLjEuMTc0NTc3MDU2Ni4wLjAuMA..

# Inspiration
In a world where distractions and the rush of life make true connections feel fleeting, RootXR reimagines how we focus and collaborate. Designed for those who find it hard to stay grounded, especially individuals with ADHD, it transforms the act of connecting into something tangible and beautiful.

# What It Does
RootXR uses AI to transform spoken words into a dynamic, living tree in mixed reality. Each branch represents an idea, growing in real time to visualize the flow of conversation. The AI analyzes how each input relates to the main topic: similar ideas grow horizontally, while new topics branch out in different directions. Ideally, users can zoom into specific branches or the trunk to view detailed summaries of the discussion. They can also manually connect ideas to build a cohesive structure. These gentle, dynamic visual cues keep everyone engaged and focused, providing structure without judgment.

# How We Built It
Our system leverages Meta’s Voice SDK for speech-to-text functionality and integrates it into a Unity-based mixed-reality application. The AI processes transcriptions in real-time, using custom parsing logic to clean and organize conversation data. Summarized text is dynamically visualized as a growing, branching tree within the mixed-reality environment. We developed two key components:

### Speech Interaction:
Managed via Meta’s Voice SDK, enabling seamless speech capture and transcription.
### Dynamic Visualization:
Built in Unity with customizable branching structures, allowing team ideas to grow and connect visually. Additionally, we integrated Photon Fusion for real-time collaboration, enabling synchronized visualization for multiple users in a shared XR space.
### Challenges We Ran Into 
Working with Meta’s Voice SDK for real-time transcription presented some difficulties, particularly in handling off-topic or noisy speech. Ensuring accurate and efficient parsing of the transcriptions required iterative improvements to the AI pipeline. Another challenge was building a tree visualization that is both intuitive and adaptable. It needed to provide structure without overwhelming users, requiring thoughtful UX and visual design iterations. Additionally, integrating Photon Fusion for multi-user support introduced challenges in maintaining synchronization across devices.

## Accomplishments That We're Proud Of
Received an Honorable Mention for the Agog Connecting for Change Social XR track and were selected to participate in the MIT Reality Hack Founder's Lab.
Developed a seamless workflow for converting speech into a dynamic, growing tree that visualizes team conversations.
Successfully integrated real-time AI processing for speech transcription and topic summarization.
Built a collaborative mixed-reality space powered by Photon Fusion, allowing multiple users to interact in real-time.
Designed an inclusive tool tailored to support both neurodivergent and neurotypical team members in improving focus and collaboration.
Root is more than a tool; it’s a shared experience. It celebrates the beauty of diverse thinking and fosters an environment where creativity flows, and collaboration thrives. Together, we create something greater than the sum of its parts, a workspace where focus feels natural and every contribution matters.

With RootXR, the connection grows, creativity flourishes, and we stay rooted together.

