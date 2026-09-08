# Command AI and operational planning references

## Included: PyCMO

Source: [duyminh1998/pycmo](https://github.com/duyminh1998/pycmo)

PyCMO is MIT-licensed and is preserved as a focused learning reference under
`Tools/ThirdParty/PyCMO-Learning/`.

The included source and tests demonstrate useful patterns for OPAW:

- Separating observations, available actions, rewards, and episode state
- Converting scenario state into stable feature vectors
- Keeping agent implementations behind a common interface
- Supporting scripted, rule-based, random, and reinforcement-learning agents
- Running an observation/action loop through a protocol boundary
- Testing action spaces, feature extraction, protocol handling, and terminal state

This is not wired into Unity and does not connect OPAW to Command: Modern Operations. Its value is architectural: OPAW can adapt the observation/action/reward separation for autonomous commanders, simulation evaluation, training scenarios, and after-action analysis.

The original MIT license and README are included with the reference.

## Reviewed but not imported

### Command_Resources

Source: [GrandStrategos/Command_Resources](https://github.com/GrandStrategos/Command_Resources)

This is GPL-3.0 material for Command: Modern Operations, including Lua scripts, scenario imports, zones, spreadsheets, PDFs, and other scenario-specific content. It may be useful as external inspiration for sensor, zone, and scenario-authoring workflows, but its files and CMO-specific content are not copied into OPAW.

### cmo_debug

Source: [musurca/cmo_debug](https://github.com/musurca/cmo_debug)

This is GPL-3.0 Lua tooling for editing Command: Modern Operations scenarios. Its ideas around batch operations, coordinate lookup, group movement, fuel control, and debugging are relevant to OPAW tooling, but the source is not imported because of the copyleft license and CMO-specific API.

### CMO_Intellisense

Source: [blu3ser/CMO_Intellisense](https://github.com/blu3ser/CMO_Intellisense)

This repository provides a Lua API completion library for Command: Modern Operations. No clear repository license was found, so its files are not copied. OPAW should build its own typed command and telemetry schema rather than depend on this API library.

## OPAW adaptations

The transferable design direction is:

- `ObservationSnapshot`: normalized battlefield state, sensor confidence, logistics, morale, and communications health
- `ActionProposal`: commander intent with constraints, priority, time horizon, and authority scope
- `RewardLedger`: mission progress, force preservation, supply efficiency, information advantage, and civilian-risk penalties
- `Episode/OperationState`: deterministic scenario lifecycle, terminal conditions, replay checkpoints, and after-action metrics
- `CommandProtocol`: validated messages between strategic, operational, and tactical autonomy layers

These concepts must be implemented with OPAW-native types and systems rather than copied CMO code or assets.
